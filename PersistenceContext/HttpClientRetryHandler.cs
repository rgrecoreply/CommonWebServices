using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace PersistenceContext
{
    public class HttpClientRetryHandler : DelegatingHandler
    {
        public HttpClientRetryHandler(HttpClientHandler handler) : base(handler) { }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            /*I normali criteri di ripetizione possono influire sul sistema quando scalabilità
            e concorrenza sono elevate e sono presenti molti conflitti. 
            Per risolvere i picchi di tentativi simili provenienti da molti client in caso 
            di interruzioni parziali, una soluzione alternativa efficace consiste 
            nell'aggiungere una strategia di instabilità all'algoritmo o ai criteri di ripetizione. 
            Ciò può migliorare le prestazioni complessive del sistema end - to - end 
            grazie all'aggiunta di casualità nel backoff esponenziale. 
            Permette di diluire i picchi quando si verificano problemi.
            */

            Random jitterer = new Random();

            return Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult<HttpResponseMessage>(x =>
                    x.StatusCode == System.Net.HttpStatusCode.GatewayTimeout ||
                    x.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(3, // exponential back-off plus some jitter
                                      retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)))
                .ExecuteAsync(() => base.SendAsync(request, cancellationToken));
        }
    }
}
