USE [msdb]
GO

/****** Object:  Job [jBoxDLSaleFlowProcess]    Script Date: 2017-7-19 12:15:31 ******/
IF  EXISTS (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE name = N'jBoxDLSaleFlowProcess')
EXEC msdb.dbo.sp_delete_job @job_id=N'025a7ca0-385a-4c24-be44-229777ac1f02', @delete_unused_schedule=1
GO

/****** Object:  Job [jBoxDLSaleFlowProcess]    Script Date: 2017-7-19 12:15:31 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 2017-7-19 12:15:31 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
select @jobId = job_id from msdb.dbo.sysjobs where (name = N'jBoxDLSaleFlowProcess')
if (@jobId is NULL)
BEGIN
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'jBoxDLSaleFlowProcess', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=2, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'大连客户，电子秤销售数据自动处理', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'NT SERVICE\SQLSERVERAGENT', 
		@notify_email_operator_name=N'BoxEmailOperator', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END
/****** Object:  Step [1]    Script Date: 2017-7-19 12:15:31 ******/
IF NOT EXISTS (SELECT * FROM msdb.dbo.sysjobsteps WHERE job_id = @jobId and step_id = 1)
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'1', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
		
		

	BEGIN TRY
		EXEC	[dbo].[pBoxElectronicScaleDataProcessing]
				@vGenerateSaleFlow = N''Y'',
				@vGenerateDO = N''N'',
				@vGeneratePI = N''N''

	END TRY
	BEGIN CATCH



			DECLARE @ErrorMessage NVARCHAR(4000);  
			DECLARE @ErrorSeverity INT;  
			DECLARE @ErrorState INT;  

			SET @ErrorMessage =  ''ERROR_PROCEDURE: '' + ERROR_PROCEDURE() 
								+ ''; '' + char(13) + char(10) + ''ErrorLine: '' + cast(ERROR_LINE() as varchar(50)) 
								+ ''; '' + char(13) + char(10) + ''ERROR_MESSAGE: '' + ERROR_MESSAGE()  
								+ ''; '' + char(13) + char(10);  
			SET @ErrorSeverity = ERROR_SEVERITY();  
			--SET @ErrorSeverity = 16; 
			SET @ErrorState = ERROR_STATE();  



			--存在错误时，自动发送邮件给联系人
			declare @vBody nvarchar(MAX)
			select @vBody =  ''存在错误！'' + char(13) + char(10) + @ErrorMessage;
			exec pBoxSendMessageFromText @vBody;



	END CATCH;', 
		@database_name=N'eShop4', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'每个一段时间执行', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20170719, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'be21994a-636e-4b8b-a998-1abdacb502f1'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO

