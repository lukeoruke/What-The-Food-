To run the application please be using Visual Studio 2022.
Have MariaDB 10.6 installed. With the ID set to 'root' and the password set to 'myPassword
If you wish to change the connection string for whatever reason it is located in the file 'context.cs'
Run the file Program.cs to get a command line proof of concept
A default admin account is created on program start and persisted on the DB if an admin account is not already present
Login: Admin
Password: pass

There is a known error of the application crashing when exiting the command line interface. It is caused by a deprecated function call

