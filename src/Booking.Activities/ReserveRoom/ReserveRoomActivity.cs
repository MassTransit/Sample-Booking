namespace Booking.Activities.ReserveRoom
{
    using System;
    using System.Threading.Tasks;
    using MassTransit;
    using MassTransit.Courier;


    public class ReserveRoomActivity :
        Activity<ReserveRoomArguments, ReserveRoomLog>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<ReserveRoomArguments> context)
        {
            await Task.Delay(1000);

            var reservationId = NewId.NextGuid().ToString("N");

            await Console.Out.WriteLineAsync($"Booking room {reservationId} using {context.Arguments.ReservationApiKey}");

            return context.Completed(new Log(reservationId));
        }

        public async Task<CompensationResult> Compensate(CompensateContext<ReserveRoomLog> context)
        {
            await Console.Out.WriteLineAsync($"Cancelling reservationId: {context.Log.ReservationId}");

            return context.Compensated();
        }


        class Log :
            ReserveRoomLog
        {
            public Log(string reservationId)
            {
                ReservationId = reservationId;
            }

            public string ReservationId { get; }
        }
    }
}