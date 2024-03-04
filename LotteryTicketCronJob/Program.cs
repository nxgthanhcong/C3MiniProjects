using LotteryTicketCronJob;
using Quartz.AspNetCore;
using Quartz;
using LotteryTicketCrawler.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ILotteryTicketService, LotteryTicketService>();
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("LotteryTicketJob");
            q.AddJob<LotteryTicketJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("LotteryTicketJob-trigger")
                .StartNow()
            );
        });

        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
    })
    .Build();

await host.RunAsync();
