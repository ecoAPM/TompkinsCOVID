using TompkinsCOVID;

await Factory.App().Run(args.Any() ? args[0] : null);