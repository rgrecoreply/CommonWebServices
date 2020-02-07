using CRMWebCommon.Entities;
using CRMWebCommon.Helpers;
using Services.EntityInterfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDecorator.EntityDecorators
{
    public class AppointmentServiceDecorator : BaseDecorator<Appointment>, IAppointmentService
    {
        private readonly string _cacheKey = "appointment";

        public AppointmentServiceDecorator(IService<Appointment> service) : base(service) { }

        public override IEnumerable<Appointment> GetAll()
        {
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(_cacheKey))
            {
                return cache.Get(_cacheKey) as List<Appointment>;
                ;
            }
            else
            {
                List<Appointment> appointments = base.GetMultiple(QueryHelper<Appointment>.CreateInstance().AddColumns(x => x.AdditionalParameters, x => x.StatusCode)).ToList();
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 1, 0, 0).AddDays(1));
                cache.Add(_cacheKey, appointments, policy);

                return appointments;
            }
        }

        public override IEnumerable<Appointment> GetMultiple(QueryHelper<Appointment> query)
        {
            return base.GetMultiple(query);
        }

        public override bool Update(Appointment entity)
        {
            ObjectCache cache = MemoryCache.Default;

            if (cache.Contains(_cacheKey))
            {
                IList<Appointment> appointments = (cache[_cacheKey] as IList<Appointment>).Where(x => !x.Id.Equals(entity.Id)).ToList();
                appointments.Add(entity);
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(12));
                cache.Set(_cacheKey, appointments, policy);
            }

            return base.Update(entity);
        }
    }
}
