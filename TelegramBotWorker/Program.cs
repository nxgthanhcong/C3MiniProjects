using Quartz;
using Quartz.AspNetCore;
using TelegramBotWorker;
using TelegramBotWorker.QuartzJobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {

        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("DDictJob");
            q.AddJob<DDictJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("DDictJob-trigger")
                .WithCronSchedule("0 0 8,11,14,17,20 ? * * *")
                .StartNow()
            );
        });

        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.AddHostedService<Worker>();
    })
    .Build();

Console.WriteLine("app start ok");

await host.RunAsync();
