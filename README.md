

##Running in development
Running the scripts/run.sh scipt will start both the Sauve web server and the webpack devserver in paralell. By default the server will just server some simple default data. To connect to Azure, either supply a connection string and a varible called "tableName" as parameters to the server when running, or set the WINEVUE_AZURE_CONNECTION environment variable to the connectionsting, by default the server will query a table called "winestable"

