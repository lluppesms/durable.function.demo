
# Introduction 
This is an example of how to call a long running durable function, specifically tied to the durable.function.demo project. 
It will allow you to run these functions interactively and test out the functionality.

# Getting Started

---

## 1. Run the testing tool locally
1.1. Load up the project, edit the config.json file to point to your function, then run the app locally.

---

## 2. Deploy an EXE version of the testing tool
1.1. Build a Testing tool EXE by creating a pipeline based on deploy-tester-only.yml. 
That pipeline will create an artifact and can also save it out to a storage folder for future use and easy deployment.

1.2. Copy the EXE file locally, and create/edit a config.json file to have your phone number and the URL for your deployed function.

1.3. Use the Simulator to test your functions.

1.4. You can create multiple config files to point to multiple function locations (local/dev/qa/prod) and just specify the config file name as a command line parameter.
