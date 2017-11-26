

##Running in development
Running the scripts/run.sh scipt will start both the Sauve web server and the webpack devserver in paralell. By default the server will just server some simple default data. To connect to Azure, either supply a connection string and a varible called "tableName" as parameters to the server when running, or set the WINEVUE_AZURE_CONNECTION environment variable to the connectionsting, by default the server will query a table called "winestable"

Run docker container:
export WINEVUE_AZURE_CONNECTION=<connectionString>
export WINEVUE_TABLE_NAME=<tableName>
docker run -p 127.0.0.1:8083:8083 --rm --name winevuetest -e WINEVUE_TABLE_NAME -e WINEVUE_AZURE_CONNECTION -it sp1nakr/winevue

