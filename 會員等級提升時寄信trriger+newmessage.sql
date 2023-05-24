USE [BookShop]
GO
CREATE TRIGGER [dbo].[MemberLevelUpgrade]
ON [dbo].[Member]
AFTER UPDATE
AS
BEGIN
    IF UPDATE(LevelID) -- �u�bLevelID����s��Ĳ�o
    BEGIN
        -- �ˬd�s�����ŬO�_�j���ª�����
        IF (SELECT LevelID FROM inserted) > (SELECT LevelID FROM deleted)
        BEGIN
            -- �s���|������ID
            DECLARE @newLevelID INT;
            SET @newLevelID = (SELECT LevelID FROM inserted);

            -- �ھڷs���|������ID�A������������T��ID
            DECLARE @messageID INT;
            SET @messageID = CASE
                WHEN @newLevelID = 2 THEN 10 -- ��ַ|��
                WHEN @newLevelID = 3 THEN 11 -- �եַ|��
                WHEN @newLevelID = 4 THEN 12 -- ���ַ|��
                WHEN @newLevelID = 5 THEN 13 -- �p�ַ|��
                ELSE 0 -- �L�����T��
            END;

            IF @messageID > 0
            BEGIN
                -- �s�W�@���O����MessageMemberDetail��
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
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (10, 2, N'���߱z�ɵ�����ַ|���I', N'<p>�˷R���|���A</p><p>���߱z�ɵ�����ַ|���I�P�±z���[�J�M����I�@���ڭ̪���ַ|���A�z�N�ɦ�</p><p>���d��95��</p>���u�f�C<p>�ڭ̭P�O�󬰱z���ѧ�n�����~�M�A�ȡA���H�����`�z���ݨD�C�P�±z������A���ݬ��z���ѧ�h��ߩM��m������I</p>���P�z�I')
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (11, 2, N'���߱z�ɵ����եַ|���I', N'<p>�˷R���|���A</p><p>���߱z�ɵ�����ַ|���I�P�±z���[�J�M����I�@���ڭ̪���ַ|���A�z�N�ɦ�</p><p>���d��95��B���d��1%�^�X��</p>���u�f�C<p>�ڭ̭P�O�󬰱z���ѧ�n�����~�M�A�ȡA���H�����`�z���ݨD�C�P�±z������A���ݬ��z���ѧ�h��ߩM��m������I</p>���P�z�I')
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (12, 2, N'���߱z�ɵ������ַ|���I', N'<p>�˷R���|���A</p><p>���߱z�ɵ�����ַ|���I�P�±z���[�J�M����I�@���ڭ̪���ַ|���A�z�N�ɦ�</p><p>')
GO
INSERT [dbo].[Message] ([MessageID], [MessageTypeID], [MessageTitle], [MessageContent]) VALUES (13, 2, N'���߱z�ɵ����p�ַ|���I', N'<p>�˷R���|���A</p><p>���߱z�ɵ�����ַ|���I�P�±z���[�J�M����I�@���ڭ̪���ַ|���A�z�N�ɦ�</p><p>')
GO
SET IDENTITY_INSERT [dbo].[Message] OFF
GO
