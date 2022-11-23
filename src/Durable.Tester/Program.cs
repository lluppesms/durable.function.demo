var configuration = Configurator.Read(args);
if (configuration == null) return;

var testScenario = new DurableTest();
testScenario.Run(configuration);
