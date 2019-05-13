create database EventManager


go
create Table UserRoles(
UserRoleId INT NOT NULL PRIMARY KEY,
RoleName VARCHAR(10) NOT NULL
);
go

Create TABLE Events(
    EventId INT NOT NULL PRIMARY KEY,
    EventName VARCHAR(50) not NULL,
    UserId
)