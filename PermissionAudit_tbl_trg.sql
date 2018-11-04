
USE csn_dba
go

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

use AW

GO

ALTER TRIGGER trgDBPermissionAudit   
ON DATABASE   
FOR DDL_DATABASE_SECURITY_EVENTS  
AS  
-- http://wutils.com/wmi/root/microsoft/sqlserver/serverevents/mssqlserver/ddl_database_security_events/
-- Track all events in [DDL_DATABASE_SECURITY_EVENTS] group and write them into csn_dba..tblPermissionAudit
	DECLARE @ErrorMessage NVARCHAR(MAX)
	BEGIN TRY
		IF (OBJECT_ID('csn_dba..tblPermissionAudit')) IS NULL
		BEGIN
			RAISERROR ('csn_dba..tblPermissionAudit is not accessible' , 16, 1)  
		END

		INSERT INTO csn_dba..tblPermissionAudit -- Trigger will fail if user, who has invoked the permission change, has no rights to write into csn_dba..tblPermissionAudit
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
	END TRY 
	BEGIN CATCH
		SET @ErrorMessage = CONCAT('Audit Trigger failure: please investigate:',ERROR_MESSAGE()) 
		RAISERROR (@ErrorMessage , 19, 1)  WITH LOG 
	END CATCH
GO  









