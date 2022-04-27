
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[BAT_RESET_APP_INFOB_KAIIN]') AND type in (N'P'))
DROP PROCEDURE [dbo].[BAT_RESET_APP_INFOB_KAIIN]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ====================================================
-- 名称			: BAT_RESET_APP_INFOB_KAIIN
-- 機能			: INFOB_SETTING の内容で、APP_INFOB_KAIIN のレコードを再設定する。
-- 引き数		: なし
-- 戻り値		: なし
-- 作成日		: 2020/09/24  作成者 : 茅川
-- 更新			: 2022/01/07　茅川
-- ====================================================
CREATE PROCEDURE [dbo].[BAT_RESET_APP_INFOB_KAIIN]
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @TranCnt int;  
    SET @TranCnt = @@TRANCOUNT;

	IF @TranCnt > 0  
        SAVE TRANSACTION svpt_BAT_RESET_APP_INFOB_KAIIN_1;  
    ELSE 
        BEGIN TRANSACTION;  

	BEGIN TRY
		SELECT DISTINCT
			b.FOLDERNM,
			a.KIM_KAIINCD,
			b.FLAG
		INTO #wt_BAT_RESET_APP_INFOB_KAIIN_2
		FROM KAIIN_M a
		INNER JOIN #wt_BAT_RESET_APP_INFOB_KAIIN_1 b
		ON a.KIM_KAIINCD BETWEEN b.KAIINCD_FROM AND b.KAIINCD_TO;

		DELETE t1
		FROM APP_INFOB_KAIIN t1
		INNER JOIN #wt_BAT_RESET_APP_INFOB_KAIIN_2 t2
		ON t1.AIK_FOLDERNM = t2.FOLDERNM
			AND t1.AIK_KAIINCD = t2.KIM_KAIINCD;

		INSERT INTO APP_INFOB_KAIIN
		SELECT
			FOLDERNM,
			KIM_KAIINCD,
			FLAG
		FROM #wt_BAT_RESET_APP_INFOB_KAIIN_2;

		IF @TranCnt = 0
            COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @TranCnt = 0  
            ROLLBACK TRANSACTION;
		ELSE IF XACT_STATE() <> -1
			ROLLBACK TRANSACTION svpt_BAT_RESET_APP_INFOB_KAIIN_1;

		DECLARE @ErrMessage nvarchar(4000), @ErrSeverity int, @ErrState int;  
  
        SELECT @ErrMessage = ERROR_MESSAGE();  
        SELECT @ErrSeverity = ERROR_SEVERITY();  
        SELECT @ErrState = ERROR_STATE();  
  
        RAISERROR (@ErrMessage, @ErrSeverity, @ErrState ); 
	END CATCH
END

GO
