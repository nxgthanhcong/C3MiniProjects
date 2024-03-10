using Quartz;
using Quartz.AspNetCore;
using TelegramBotWorker;
using TelegramBotWorker.Listeners;
using TelegramBotWorker.QuartzJobs;
using Translation.Business.Implementions;
using Translation.Business.Interfaces;
using Translation.Models.DataModels;
using Translation.Repositories.Implementions;
using Translation.Repositories.Interfaces;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<PostgreDatabaseConfiguration>(hostContext.Configuration.GetSection(PostgreDatabaseConfiguration.Position)); 
        services.AddSingleton<IDictionaryBotListener, DictionaryBotListener>();
        services.AddSingleton<ITranslationBusiness, TranslationBusiness>();
        services.AddSingleton<ITranslationRepository, TranslationRepository>();
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("DDictJob");
            q.AddJob<DDictJob>(opts => opts.WithIdentity(jobKey));

            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("DDictJob-trigger")
                //.WithCronSchedule("0 0 8,11,14,17,20 ? * * *")
                .WithCronSchedule("0 * * ? * *")
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
await host.RunAsync();
