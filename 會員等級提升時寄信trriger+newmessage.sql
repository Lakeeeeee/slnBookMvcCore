USE [BookShop]
GO
CREATE TRIGGER [dbo].[MemberLevelUpgrade]
ON [dbo].[Member]
AFTER UPDATE
AS
BEGIN
    IF UPDATE(LevelID) -- 只在LevelID欄位更新時觸發
    BEGIN
        -- 檢查新的等級是否大於舊的等級
        IF (SELECT LevelID FROM inserted) > (SELECT LevelID FROM deleted)
        BEGIN
            -- 新的會員等級ID
            DECLARE @newLevelID INT;
            SET @newLevelID = (SELECT LevelID FROM inserted);

            -- 根據新的會員等級ID，對應到對應的訊息ID
            DECLARE @messageID INT;
            SET @messageID = CASE
                WHEN @newLevelID = 2 THEN 10 -- 綠皮會員
                WHEN @newLevelID = 3 THEN 11 -- 白皮會員
                WHEN @newLevelID = 4 THEN 12 -- 金皮會員
                WHEN @newLevelID = 5 THEN 13 -- 鑽皮會員
                ELSE 0 -- 無對應訊息
            END;

            IF @messageID > 0
            BEGIN
                -- 新增一筆記錄到MessageMemberDetail表
                INSERT INTO MessageMemberDetail (MessageID, MemberID, UpdateTime, ReadStatu)
                VALUES (
                    @messageID,
                    (SELECT MemberID FROM inserted),
                    GETDATE(),
                    0
                );
            END
        END
    END
END
go
USE [BookShop]
GO
SET IDENTITY_INSERT [dbo].[Message] ON 
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (10, 2, N'恭喜您升等為綠皮會員！', N'<p>親愛的會員，</p><p>恭喜您升等為綠皮會員！感謝您的加入和支持！作為我們的綠皮會員，您將享有</p><p>滿千享95折</p>的優惠。<p>我們致力於為您提供更好的產品和服務，並隨時關注您的需求。感謝您的支持，期待為您提供更多驚喜和精彩的體驗！</p>祝賀您！')
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (11, 2, N'恭喜您升等為白皮會員！', N'<p>親愛的會員，</p><p>恭喜您升等為綠皮會員！感謝您的加入和支持！作為我們的綠皮會員，您將享有</p><p>滿千享95折、滿千享1%回饋金</p>的優惠。<p>我們致力於為您提供更好的產品和服務，並隨時關注您的需求。感謝您的支持，期待為您提供更多驚喜和精彩的體驗！</p>祝賀您！')
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (12, 2, N'恭喜您升等為金皮會員！', N'<p>親愛的會員，</p><p>恭喜您升等為綠皮會員！感謝您的加入和支持！作為我們的綠皮會員，您將享有</p><p>')
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (13, 2, N'恭喜您升等為鑽皮會員！', N'<p>親愛的會員，</p><p>恭喜您升等為綠皮會員！感謝您的加入和支持！作為我們的綠皮會員，您將享有</p><p>')
GO
SET IDENTITY_INSERT [dbo].[Message] OFF
GO
