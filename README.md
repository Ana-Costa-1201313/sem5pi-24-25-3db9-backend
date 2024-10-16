## 1. Description of the Project

This application is a prototype system for cirurgic requests, appointment,and resource management. 

It will allow:
- hospitals and clinics to manage surgery appointments and patient records;
- real-time 3D visualization of resource availability within the facility;
- schedule optimization and resource usage;
- GDPR Regulation (EU) 2016/679 compliance, ensuring the system meets data protection and consent management requirements.

This project is part of the sem5pi-24-25-3db9-backend repository, belonging to the sem5pi-24-25-3db9 Project. The solution should be developed in 14 weeks.

## 2. Planning and Technical Documentation

[Planning and Technical Documentation](docs/readme.md)


## 3. How to Build

Run one of the following scripts in the Terminal:
- "build" script, that installs the necessary tools and husky for the pre-commit hook;
- "updateBD" script, that drops the tables and the migrations and creates them again;

*Example:* open the terminal and type ./build.bat

*Note:* This project uses .NET version 8.0.

To use the database, configure a Microsoft SQL Server database:
- for a local database, simply run the "build" and the "updateDB" scripts. A .db file will be created in your user folder.
- for the remote DEI database, install the SQL extension, create a new database and follow these steps:
  - server name = vsgate-s1.dei.isep.ipp.pt,10513
  - leave the name empty
  - choose SQL login
  - user ID and password put the ones in the Startup.cs ConfigureServices method
  - name it as you like, like BD.

*Note:* in the Startup.cs ConfigureServices method, comment the database configuration you are not using.


## 4. How to Execute Tests

Run the "runTests" script, that builds and runs the tests, saying the number of tests and possible failures.

As an alternative, click with the mouse's right button and select "run tests". 


## 5. How to Run

Run the "run" script, that builds and runs the application, giving the HTTP/s URLs;

*Example:* open the terminal and type ./run.bat