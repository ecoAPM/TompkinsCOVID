using TompkinsCOVID;

await Factory.Runner().Run(args.Any() ? args[0] : null);