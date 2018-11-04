CREATE TABLE tblPermissionAudit
(
dtTimeStamp		DateTime,
EventType		NVARCHAR(MAX),
ServerName		NVARCHAR(MAX),
LoginName		NVARCHAR(MAX),
DatabaseName	NVARCHAR(MAX),
ObjectName		NVARCHAR(MAX),
ObjectType		NVARCHAR(MAX),
RoleName		NVARCHAR(MAX),
Grantees		NVARCHAR(MAX),
Permissions		NVARCHAR(MAX),
DDLCommand		NVARCHAR(MAX)
)

GO

-- http://wutils.com/wmi/root/microsoft/sqlserver/serverevents/mssqlserver/ddl_database_security_events/

ALTER TRIGGER trgDBPermissionAudit   
ON DATABASE   
FOR DDL_DATABASE_SECURITY_EVENTS  
AS   
--IF (@@ROWCOUNT = 0)
	INSERT INTO AW..tblPermissionAudit
	SELECT
		 GetDate()
		,EVENTDATA().value('(/EVENT_INSTANCE/EventType)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/ServerName)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/LoginName)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/DatabaseName)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/ObjectType)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/RoleName)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/Grantees)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/Permissions)[1]','nvarchar(max)')  
		,EVENTDATA().value('(/EVENT_INSTANCE/TSQLCommand)[1]','nvarchar(max)')  
GO  

