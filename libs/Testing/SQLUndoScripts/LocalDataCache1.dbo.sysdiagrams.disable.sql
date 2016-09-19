IF EXISTS (SELECT * FROM sys.change_tracking_tables WHERE object_id = OBJECT_ID(N'[dbo].[sysdiagrams]')) 
   ALTER TABLE [dbo].[sysdiagrams] 
   DISABLE  CHANGE_TRACKING
GO
