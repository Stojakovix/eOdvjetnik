using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOdvjetnik.Models
{
    public class Reminder
    {
        /// <summary>
        /// Gets or sets a value that decides to notify the reminder before the appointment’s start time.
        /// </summary>
        public TimeSpan TimeBeforeStart { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the reminder is dismissed or not. 
        /// </summary>
        public bool IsDismissed { get; set; }

    }
}
