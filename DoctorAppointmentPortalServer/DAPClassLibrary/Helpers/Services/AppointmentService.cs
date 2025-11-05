using DAPClassLibrary.Helpers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPClassLibrary
{
    public class AppointmentService
    {
        public List<AvailableSlots> GetAvailableSlots(int doctorId, string requestedDate)
        {
            try
            {
                if (doctorId == 0)
                    throw new ArgumentException("Invalid DoctorId");

                if (!DateTime.TryParse(requestedDate, out DateTime requestedDateTime))
                    throw new ArgumentException("Invalid Date Format");

                DateTime today = DateTime.Today;
                DateTime maxDate = today.AddDays(14);

                if (requestedDateTime < today || requestedDateTime > maxDate)
                    throw new ArgumentException("Requested date must be within 14 days from today.");

                AppointmentsOps objAppointmentsOps = new AppointmentsOps
                {
                    DoctorId = doctorId,
                    AppointmentDate = requestedDateTime
                };

                DoctorAvailability doctorData = objAppointmentsOps.GetDoctorAvailabilityAndAcceptedSlots();

                List<DoctorAvailableSlots> doctorAvailableTimes = doctorData.AvailableSlots;
                List<Appointments> acceptedAppointments = doctorData.AcceptedAppointments;

                List<AvailableSlots> availableSlots = new List<AvailableSlots>();
                int slotCount = 0;

                foreach (var time in doctorAvailableTimes)
                {
                    TimeSpan start = TimeSpan.Parse(time.StartTime);
                    TimeSpan end = TimeSpan.Parse(time.EndTime);
                    TimeSpan slotDuration = TimeSpan.FromMinutes(30);

                    for (TimeSpan current = start; current < end; current = current.Add(slotDuration))
                    {
                        TimeSpan slotEnd = current.Add(slotDuration);

                        bool isBooked = acceptedAppointments.Any(a =>
                            TimeSpan.Parse(a.ApprovedStartTime) < slotEnd &&
                            TimeSpan.Parse(a.ApprovedEndTime) > current);

                        if (!isBooked)
                        {
                            availableSlots.Add(new AvailableSlots
                            {
                                AvailableSlotId = ++slotCount,
                                DateOfSlot = requestedDateTime,
                                StartTime = current.ToString(@"hh\:mm"),
                                EndTime = slotEnd.ToString(@"hh\:mm")
                            });
                        }
                    }
                }

                return availableSlots;
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(AppointmentService), nameof(GetAvailableSlots));
              

               
                return new List<AvailableSlots>();
            }
        }


    }
}
