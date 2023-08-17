using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.Scheduler;


namespace eOdvjetnik.Services
{
    internal class SfBehavior : Behavior<ContentPage>
    {
        private SfScheduler? scheduler;

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.scheduler = bindable.FindByName<SfScheduler>("Scheduler");
            this.scheduler.ViewChanged += this.OnSchedulerViewChanged;
        }

        private void OnSchedulerViewChanged(object? sender, SchedulerViewChangedEventArgs e)
        {
            if (e.OldView == e.NewView || this.scheduler == null)
            {
                return;
            }

            if (e.NewView == SchedulerView.TimelineMonth || e.NewView == SchedulerView.TimelineDay)
            {
                this.scheduler.TimelineView.TimeIntervalWidth = 150;
            }
            else
            {
                this.scheduler.TimelineView.TimeIntervalWidth = 50;
            }
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            base.OnDetachingFrom(bindable);
            if (this.scheduler != null)
            {
                this.scheduler.ViewChanged -= this.OnSchedulerViewChanged;
                this.scheduler = null;
            }
        }
    }
}
