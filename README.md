# Zip Storage System


Solution composed of a backend web application system, written in .NET 7, and a frontend page written in NextJS (ReactJS).

The backend system is responsible for uploading ZIP files in a local folder storage, validating its structure, deleting previously uploaded files and listing the folder structure.

The frontend page can be used to manage the zip files, uploading, validating, deleting and visualizing the zip files previously uploaded.

## Requirements
- .NET Core 7
- Node Version ^18 

## Usage
The system was developed and designed to be used in a local environment. it is **not production-ready**

Clone the repository to your machine
```shell
git clone https://github.com/b-rantes/zip-storage-service.git
```

### Backend
A more detailed explanation of each route can be found at the swagger doc
- Open the solution under the **./ZipStorageService** folder.
- Ensure that a folder called **zips** is created in the **same directory** of your SolutionFile (the zip files will be stored there - the folder must be previously created)
- Set **WebApi.csproj** as the startup project
- In **launch.settings.json** launch the app preferably **in port 5000**
- Access the **swagger** in your browser at **/swagger**

###Frontend
Open the folder in your command line
```shell
cd ./zip-storage-service/ZipStorageClient/zip-storage-frontend
```

Install dependencies
```shell
npm install
```

Ensure that file **./zip-storage-frontend/pages/api/http-common.js** is pointing to the **same port** and address (http or https) that the backend is running

Run with
```shell
npm run dev
```
and the page should be launched in **localhost:3000/**

## Notes
- The backend must be running to frontend work properly
- The zips folder must exist previously
- The solution was not prepared to run with docker, given the scope of the proposed solution
