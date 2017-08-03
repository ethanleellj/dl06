--http://www.snapdba.com/2013/04/enabling-and-configuring-database-mail-in-sql-server-using-t-sql/



USE master
GO
sp_configure 'show advanced options',1
GO
RECONFIGURE WITH OVERRIDE
GO
sp_configure 'Database Mail XPs',1
GO
RECONFIGURE 
GO

USE msdb
GO

/****** Object:  MailProfile BoxEmailProfile    Script Date: 2015-9-12 16:17:03 ******/
EXEC msdb.dbo.sysmail_delete_profile_sp @profile_name=N'BoxEmailProfile', @force_delete=True

/****** Object:  MailAccount BoxEmailAccount    Script Date: 2015-9-12 16:18:24 ******/
EXEC msdb.dbo.sysmail_delete_account_sp @account_name=N'BoxEmailAccount'

/****** Object:  Operator [BoxEmailOperator]    Script Date: 2015-9-12 16:19:36 ******/
EXEC msdb.dbo.sp_delete_operator @name=N'BoxEmailOperator'




EXECUTE msdb.dbo.sysmail_add_profile_sp
@profile_name = 'BoxEmailProfile',
@description = 'Profile for sending Automated DBA Notifications'
GO


--EXECUTE msdb.dbo.sysmail_add_account_sp
--@account_name = 'BoxEmailAccount',
--@description = 'Account for Automated DBA Notifications',
--@email_address = 'boxuantest2@163.com',
--@display_name = '数据库邮件通知',
--@mailserver_name = 'smtp.163.com',
--@port = 25,
--@username = 'boxuantest2@163.com',
--@password =   'oyvgrvroogritdqs'

EXECUTE msdb.dbo.sysmail_add_account_sp
@account_name = 'BoxEmailAccount',
@description = 'Account for Automated DBA Notifications',
@email_address = 'test@boxuantech.com',
@display_name = '数据库邮件通知',
@mailserver_name = 'smtp.mxhichina.com',
@port = 25,
@username = 'test@boxuantech.com',
@password =   'BOxuan123456'

GO

EXECUTE msdb.dbo.sysmail_add_profileaccount_sp
@profile_name = 'BoxEmailProfile',
@account_name = 'BoxEmailAccount',
@sequence_number = 1
GO


/*
USE msdb
GO
EXEC master.dbo.xp_instance_regwrite
N'HKEY_LOCAL_MACHINE',
N'SOFTWARE\Microsoft\MSSQLServer\SQLServerAgent',
N'UseDatabaseMail',
N'REG_DWORD', 1
EXEC master.dbo.xp_instance_regwrite
N'HKEY_LOCAL_MACHINE',
N'SOFTWARE\Microsoft\MSSQLServer\SQLServerAgent',
N'DatabaseMailProfile',
N'REG_SZ',
N'BoxEmailProfile'
*/



--EXECUTE msdb.dbo.sp_send_dbmail
--@profile_name = 'BoxEmailProfile',
--@recipients = '2405677215@qq.com',
--@Subject = 'Test Message generated from SQL Server Database Mail',
--@Body = 'This is a test message from SQL Server Database Mail，我是真的测试啦33'
--GO

--运行到这里，或许你应该重启数据库，如果你没有收到邮件的话

EXEC msdb.dbo.sp_add_operator @name=N'BoxEmailOperator', 
@enabled=1, 
@weekday_pager_start_time=0, 
@weekday_pager_end_time=235959, 
@saturday_pager_start_time=0, 
@saturday_pager_end_time=235959, 
@sunday_pager_start_time=0, 
@sunday_pager_end_time=235959,
@pager_days=127, 
@email_address=N'2405677215@qq.com', 
@category_name=N'[Uncategorized]'
GO




---------------------------------------------------
/*
--下面这句，给所有的job添加通知对象，所以，应该是等到job添加完成后再执行比较好。

DECLARE @JobName SYSNAME, @JobID UNIQUEIDENTIFIER, @NotifyLevel INT, @SQL NVARCHAR(3000)

DECLARE job_operator_cursor CURSOR FOR
SELECT name, job_id, notify_level_email FROM msdb.dbo.sysjobs_view 

OPEN job_operator_cursor
FETCH NEXT FROM job_operator_cursor INTO @JobName, @JobID, @NotifyLevel
WHILE @@FETCH_STATUS = 0
BEGIN
IF NOT EXISTS(SELECT 1 FROM msdb.dbo.sysjobs_view WHERE notify_level_email = 2 and name LIKE @JobName)
BEGIN
PRINT ''
SELECT @SQL = 'EXEC msdb.dbo.sp_update_job @job_name=N'''+@JobName+''',
@notify_level_email=2,
@notify_level_netsend=2,
@notify_level_page=2,
@notify_email_operator_name=N''BoxEmailOperator'''
PRINT @SQL
EXEC sp_executesql @SQL
END
FETCH NEXT FROM job_operator_cursor INTO @JobName, @JobID, @NotifyLevel
END

CLOSE job_operator_cursor
DEALLOCATE job_operator_cursor

*/



