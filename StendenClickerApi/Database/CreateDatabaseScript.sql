﻿DECLARE @CurrentMigration [nvarchar](max)

IF object_id('[dbo].[__MigrationHistory]') IS NOT NULL
    SELECT @CurrentMigration =
        (SELECT TOP (1) 
        [Project1].[MigrationId] AS [MigrationId]
        FROM ( SELECT 
        [Extent1].[MigrationId] AS [MigrationId]
        FROM [dbo].[__MigrationHistory] AS [Extent1]
        WHERE [Extent1].[ContextKey] = N'StendenClickerApi.Migrations.Configuration'
        )  AS [Project1]
        ORDER BY [Project1].[MigrationId] DESC)

IF @CurrentMigration IS NULL
    SET @CurrentMigration = '0'

IF @CurrentMigration < '202103171136373_Full'
BEGIN
    CREATE TABLE [dbo].[Bosses] (
        [BossId] [int] NOT NULL IDENTITY,
        [BossName] [nvarchar](max),
        [BaseHealth] [int] NOT NULL,
        [BossAssetRefId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Bosses] PRIMARY KEY ([BossId])
    )
    CREATE INDEX [IX_BossAssetRefId] ON [dbo].[Bosses]([BossAssetRefId])
    CREATE TABLE [dbo].[ImageAssets] (
        [AssetId] [int] NOT NULL IDENTITY,
        [ImageDescription] [nvarchar](max),
        [Base64Image] [nvarchar](max),
        CONSTRAINT [PK_dbo.ImageAssets] PRIMARY KEY ([AssetId])
    )
    CREATE TABLE [dbo].[Heroes] (
        [HeroId] [int] NOT NULL IDENTITY,
        [HeroName] [nvarchar](max),
        [HeroInformation] [nvarchar](max),
        [HeroCost] [int] NOT NULL,
        [HeroAssetRefId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Heroes] PRIMARY KEY ([HeroId])
    )
    CREATE INDEX [IX_HeroAssetRefId] ON [dbo].[Heroes]([HeroAssetRefId])
    CREATE TABLE [dbo].[PlayerHeroes] (
        [PlayerRefId] [int] NOT NULL,
        [HeroRefId] [int] NOT NULL,
        [UnlockedTimestamp] [nvarchar](max),
        CONSTRAINT [PK_dbo.PlayerHeroes] PRIMARY KEY ([PlayerRefId], [HeroRefId])
    )
    CREATE INDEX [IX_PlayerRefId] ON [dbo].[PlayerHeroes]([PlayerRefId])
    CREATE INDEX [IX_HeroRefId] ON [dbo].[PlayerHeroes]([HeroRefId])
    CREATE TABLE [dbo].[Players] (
        [PlayerId] [int] NOT NULL IDENTITY,
        [PlayerGuid] [nvarchar](max),
        [PlayerName] [nvarchar](max),
        [DeviceId] [nvarchar](max),
        [ConnectionId] [nvarchar](max),
        CONSTRAINT [PK_dbo.Players] PRIMARY KEY ([PlayerId])
    )
    CREATE TABLE [dbo].[Friendships] (
        [Player1RefId] [int] NOT NULL,
        [Player2RefId] [int] NOT NULL,
        [Player_PlayerId] [int],
        [Player_PlayerId1] [int],
        CONSTRAINT [PK_dbo.Friendships] PRIMARY KEY ([Player1RefId], [Player2RefId])
    )
    CREATE INDEX [IX_Player1RefId] ON [dbo].[Friendships]([Player1RefId])
    CREATE INDEX [IX_Player2RefId] ON [dbo].[Friendships]([Player2RefId])
    CREATE INDEX [IX_Player_PlayerId] ON [dbo].[Friendships]([Player_PlayerId])
    CREATE INDEX [IX_Player_PlayerId1] ON [dbo].[Friendships]([Player_PlayerId1])
    CREATE TABLE [dbo].[MultiPlayerSessions] (
        [SessionId] [int] NOT NULL IDENTITY,
        [Player1RefId] [int] NOT NULL,
        [Player2RefId] [int] NOT NULL,
        [Player3RefId] [int] NOT NULL,
        [Player4RefId] [int] NOT NULL,
        [Player_PlayerId] [int],
        [Player_PlayerId1] [int],
        [Player_PlayerId2] [int],
        [Player_PlayerId3] [int],
        CONSTRAINT [PK_dbo.MultiPlayerSessions] PRIMARY KEY ([SessionId])
    )
    CREATE INDEX [IX_Player1RefId] ON [dbo].[MultiPlayerSessions]([Player1RefId])
    CREATE INDEX [IX_Player2RefId] ON [dbo].[MultiPlayerSessions]([Player2RefId])
    CREATE INDEX [IX_Player3RefId] ON [dbo].[MultiPlayerSessions]([Player3RefId])
    CREATE INDEX [IX_Player4RefId] ON [dbo].[MultiPlayerSessions]([Player4RefId])
    CREATE INDEX [IX_Player_PlayerId] ON [dbo].[MultiPlayerSessions]([Player_PlayerId])
    CREATE INDEX [IX_Player_PlayerId1] ON [dbo].[MultiPlayerSessions]([Player_PlayerId1])
    CREATE INDEX [IX_Player_PlayerId2] ON [dbo].[MultiPlayerSessions]([Player_PlayerId2])
    CREATE INDEX [IX_Player_PlayerId3] ON [dbo].[MultiPlayerSessions]([Player_PlayerId3])
    CREATE TABLE [dbo].[Upgrades] (
        [UpgradeId] [int] NOT NULL IDENTITY,
        [UpgradeName] [nvarchar](max),
        [UpgradeCost] [int] NOT NULL,
        [UpgradeIsAbility] [bit] NOT NULL,
        [HeroRefId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Upgrades] PRIMARY KEY ([UpgradeId])
    )
    CREATE INDEX [IX_HeroRefId] ON [dbo].[Upgrades]([HeroRefId])
    CREATE TABLE [dbo].[Monsters] (
        [MonsterId] [int] NOT NULL IDENTITY,
        [MonsterName] [nvarchar](max),
        [BaseHealth] [int] NOT NULL,
        [MonsterAssetRefId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Monsters] PRIMARY KEY ([MonsterId])
    )
    CREATE INDEX [IX_MonsterAssetRefId] ON [dbo].[Monsters]([MonsterAssetRefId])
    CREATE TABLE [dbo].[Scenes] (
        [SceneId] [int] NOT NULL IDENTITY,
        [SceneName] [nvarchar](max),
        [SceneAssetRefId] [int] NOT NULL,
        CONSTRAINT [PK_dbo.Scenes] PRIMARY KEY ([SceneId])
    )
    CREATE INDEX [IX_SceneAssetRefId] ON [dbo].[Scenes]([SceneAssetRefId])
    ALTER TABLE [dbo].[Bosses] ADD CONSTRAINT [FK_dbo.Bosses_dbo.ImageAssets_BossAssetRefId] FOREIGN KEY ([BossAssetRefId]) REFERENCES [dbo].[ImageAssets] ([AssetId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Heroes] ADD CONSTRAINT [FK_dbo.Heroes_dbo.ImageAssets_HeroAssetRefId] FOREIGN KEY ([HeroAssetRefId]) REFERENCES [dbo].[ImageAssets] ([AssetId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[PlayerHeroes] ADD CONSTRAINT [FK_dbo.PlayerHeroes_dbo.Heroes_HeroRefId] FOREIGN KEY ([HeroRefId]) REFERENCES [dbo].[Heroes] ([HeroId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[PlayerHeroes] ADD CONSTRAINT [FK_dbo.PlayerHeroes_dbo.Players_PlayerRefId] FOREIGN KEY ([PlayerRefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_dbo.Friendships_dbo.Players_Player1RefId] FOREIGN KEY ([Player1RefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_dbo.Friendships_dbo.Players_Player2RefId] FOREIGN KEY ([Player2RefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_dbo.Friendships_dbo.Players_Player_PlayerId] FOREIGN KEY ([Player_PlayerId]) REFERENCES [dbo].[Players] ([PlayerId])
    ALTER TABLE [dbo].[Friendships] ADD CONSTRAINT [FK_dbo.Friendships_dbo.Players_Player_PlayerId1] FOREIGN KEY ([Player_PlayerId1]) REFERENCES [dbo].[Players] ([PlayerId])
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player1RefId] FOREIGN KEY ([Player1RefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player2RefId] FOREIGN KEY ([Player2RefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player3RefId] FOREIGN KEY ([Player3RefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player4RefId] FOREIGN KEY ([Player4RefId]) REFERENCES [dbo].[Players] ([PlayerId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player_PlayerId] FOREIGN KEY ([Player_PlayerId]) REFERENCES [dbo].[Players] ([PlayerId])
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player_PlayerId1] FOREIGN KEY ([Player_PlayerId1]) REFERENCES [dbo].[Players] ([PlayerId])
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player_PlayerId2] FOREIGN KEY ([Player_PlayerId2]) REFERENCES [dbo].[Players] ([PlayerId])
    ALTER TABLE [dbo].[MultiPlayerSessions] ADD CONSTRAINT [FK_dbo.MultiPlayerSessions_dbo.Players_Player_PlayerId3] FOREIGN KEY ([Player_PlayerId3]) REFERENCES [dbo].[Players] ([PlayerId])
    ALTER TABLE [dbo].[Upgrades] ADD CONSTRAINT [FK_dbo.Upgrades_dbo.Heroes_HeroRefId] FOREIGN KEY ([HeroRefId]) REFERENCES [dbo].[Heroes] ([HeroId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Monsters] ADD CONSTRAINT [FK_dbo.Monsters_dbo.ImageAssets_MonsterAssetRefId] FOREIGN KEY ([MonsterAssetRefId]) REFERENCES [dbo].[ImageAssets] ([AssetId]) ON DELETE CASCADE
    ALTER TABLE [dbo].[Scenes] ADD CONSTRAINT [FK_dbo.Scenes_dbo.ImageAssets_SceneAssetRefId] FOREIGN KEY ([SceneAssetRefId]) REFERENCES [dbo].[ImageAssets] ([AssetId]) ON DELETE CASCADE
    CREATE TABLE [dbo].[__MigrationHistory] (
        [MigrationId] [nvarchar](150) NOT NULL,
        [ContextKey] [nvarchar](300) NOT NULL,
        [Model] [varbinary](max) NOT NULL,
        [ProductVersion] [nvarchar](32) NOT NULL,
        CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY ([MigrationId], [ContextKey])
    )
    INSERT [dbo].[__MigrationHistory]([MigrationId], [ContextKey], [Model], [ProductVersion])
    VALUES (N'202103171136373_Full', N'StendenClickerApi.Migrations.Configuration',  0x1F8B0800000000000400ED5DDB6EE4B8117D0F907F68F45312CCBADD170489D1DEC5AC3DB36B64E782E99945DE0CB99BF608AB963A927A6023C897E5219F945F0875A7C82A5E24EAD28EB1C0AC5B248BC5E261896495AAFEFBEFFFAC7F78DC7B936F248CDCC0BF9CCECFCEA713E26F839DEB3F5C4E8FF1FD777F99FEF0FDEF7FB77EB3DB3F4E7E2DEA2D937AB4A51F5D4EBFC6F1E162368BB65FC9DE89CEF6EE360CA2E03E3EDB06FB99B30B668BF3F3BFCEE6F319A124A694D664B2FE74F463774FD21FF4E755E06FC9213E3ADEBB6047BC287F4E4B3629D5C97B674FA283B32597D34D4CFC1DF1AF3C77FB1B095F1FDCB36B2776EE9C884C27AF3DD7A11C6D88773F9D38BE1FC44E4CF9BDF812914D1C06FEC3E6401F38DEE7A703A1F5EE1D2F69958EE3A2AAAE3BA4F34532A459D5B020B53D4671B03724385FE6329AF1CD1B497A5ACA904AF10D9576FC948C3A95E4E5F4C7208AA613BEA38B2B2F4C2AC9847C96347D35112ABC2AC1413194FCF76A7275F4E263482E7D728C43C77B35F978BCA3F5FF469E3E07BF11FFD23F7A1ECB26659496D51ED0471FC3E040C2F8E913B96798BFD94D27B37ADB19DFB86CCAB5CB8678E3C7CBC574F29E32E1DC79A44403238E4D1C84E427E293D089C9EEA313C724A4937943079EC853E000E82FF9ABE8914290AEAAE9E49DF3F80BF11FE2AF9753FAE774F2D67D24BBE249CEC517DFA58B90368AC323517644E7E567E278F157D5E0D40CBF8E22125359AB05C5D17AEF7C731F52B96154A7934FC44B6B445FDD43B64C5338DD3255DE86C1FE53E0E5EDAA92DB4D700CB7892C03B0F8B3133E24ED59AED6B30AF8D2E570B3771E48CE40A3455111187669A42C34591B65C3BE16472AB16B126D43F79029B91E16C99F5769B7F6FB92829F44ED905F401B417EB1307459FA998401C25252749BFC23B0542F1158E28A4D597A47B9A0F08199CA0B6FF3FF0BAC41E5028360255336375B0A7798C9B4E836FD5760902F1398132A408C692BB3641A1AAAB1A4E9B00A2CE1A089FE2ADAF5A5BE92FE7A79B7A703F3EF8370EFF4A22693FEAE82286EB793285581D59D04A399DA282F7E2781E8365DAE3E7ACE13A6BBB2B2B2039629AE48500A7CB9A9B2FA7278089D1DA2AEF2428129F6B9C051ADB0958AAAC6D650515504865557191F39C8677AEA0DACADD670B5AEDAAE4D0B64BEF85E4005BEFB4CCFF151ECEC0F3DEEAA32E4B45B6FBC16C0D6A3991A507155D402F9CAFE947196D76875D8295868B1F0C6B0E89AAFA13EF709598F3F1DDD5DE76FEEACAB5E3625D7E49BBB2537DD8FE92AF07DB24D26A28BCED0B5FC367429BC93151CCD252BFAB65E8F5FD3B5626455D7EB98EA1CA6F14293CF859CCF85069F0B633E25474E43DD28D92571DA53FB5047A2446DC9279AA92448AF2CC344575530955BD1523AB94C259437745AAB0A4D795BEAF0B694F0B654F1B66CCCDB4A87B79584B7958AB755BB7771B5AA1ABE8F2B02637827CFF577C2598345ABCDF0DCC236B6CE879D736ACE1D08BE6AC26ECB6A15FCC45201804095665B5558A908E41752FE44C5025469B544DE51C4BA19A57CD1355C2A22A161974CCE4413F8334DFBDDC80EBCE2404A4B6B9456BD6901118B9036C06B09AB4E52D5A67640BB5968F12D6A0B49D5667CC3DB11B49BA516DFE2164552B519DFF05605ED66A5C5B7B87D91546DA5A5F3BBC186AA396F3DAC3ECE9968A28F99A67DE9E3BCCB5E8EFB795FEDCD02859CA2D777AE970E32A3F66340C1E9F8BD5D659A5F2C6A5D98F38B0DBC4D6FB609CACC964D773E59EB619757CE4493E5C534ED6B79E55D9E94FB0E6BDBB6BA2AEA96775B267AE1D524B3E3375A35A945BDE19A49DB0E7C40485868743C281AF6B55AD20E7B592B9593845584B38E1B6DBD3B785CA3EE1F3A98A62D82AD9BF2C3BCA21833777D806FFCDD446EF3CEA6A7B097D3194A3684070A57DAF9E5F44F82C8508AE54D6C459175A3ABD39D4F79B87FF0AF8947623279BDCD5C64AF9C689BEE2079B4530EEA4FE8CC933001A6E35D25BA22745C3F169793EB6FDD83E349B9E75A99F8D425AC959DF025D7E490EA8D583A1B3ABDF39E142213655F9CDC54625ACF1870C931C79B553188A036D60A23ACE95F1F7BA8B3841CCEC3C30E615C77E65BC20E990FDDDE87841C704F8A814376695AE183BD6AD7079EE4B65500F5F8D08773AF0301CCCE6E04417C72F45940CD0A830071A10FC44587405C9C341017C30371610E44D85CD5EB4BB8EE73207F5F220E082AB89C9F9DCD95EF62D871A109CC5B89005F8C52DF06EB22005763172290580E306E75CC0815D390BD4D5F4D69D8214E405DA947D19BDA524FDE89BC4725A6236330812BB923E09ED47B563D8AE1817B6AEF5D89EDD0184CCB1E81BB7C16C05D8E07B84B73E02E4709DC953970573D0277F52C80BB1A0F7057E6C05D8DE0A45339DD2AF6E18007AE9D0DBEE8B9DB06FDCD25A03CE5007EBE762500EE827A9400FEAEE52B762501F075DAA30470A5CD57EC4A02A05EEE5E02ECB7011AF68182E90E4C0F39E9137A3BA1CCF7F6524267469F83215F4535BF1F0C22B01350858ED2394E1F75F087B863B776415CEBCCB30553173407BA5D0FBA49875C67D02DB3D48F8651CA85BB98C16E5C1A45E3346CFCB231E860C186A55F36453A3C004E5C83E0527079C18083FBBF54A0C91DB1F4D188874C390D2462FCF785426C5274FA175CAC06C11F171D09030A1624AC824916EA4E1F7A58F0A5D3001ECC7D5FB0836743A7773EF45C0F98CB1CEE689B98B6206181FE9AC367155A527022FD1291DC8F34CA5D17794425E43724E62281557E7E35800A78ACB7AE3007916011A920547C8A2DD0C876948AD6D56E1EA4C19EB0B428E144940418E32B408435BD2A0815C75B800A74B85550ABC2EE08D4CA63808244150D4D64A8D8D8A9C694472A1308E46F62AE39B342EA4861E3293195E0884BFC92553AA3965C57B014D6BDD2FF9421525B24BC7EAD8F5163FCBCD3232000A95F24768B503B2532DCD7D79644109833A4529C0D84007DAE2CCA41E5AC57635FE2AEC78CA0B6BC25C29038E809B2ED44200B2D812C4C04B2B02A9045B70281DC92D0A522F15E826E1E41FF25F52894A4008148C46B4528204C94FE4C8A9180406926141025768522FBE859948DAEA353FD1643EDEAC40CAF7AF34B44A5E1D1D4C9BA927D6A6D202D19A034FC6BAC49AB632D24FBC0DB405ACB26D25ADA97D6722069AD8CA4B56A22AD957D69AD7A79C33131A0504D8E98AC6556B5B62F36D146AD21D7F65290BDCF10B3B58CFBB66F32D14EDD8B1440ED22375DCBB8073549032980EAA33B29805A436EBE96710F6A88065200D5827529D442D7498F88900D1B3BD87156EC96C744CE70DD8996AC47481005815B52D5B65486E1EA7E453274D07ADAC5F118FEF01D78852AAD7CFA763E661CD54D91ECB529B3EC75767B227E322D4A456E63D2B332B14B3BBFF492C802B52B7526073ECB83280599A543C7D6C1B05EDC314B248098375A8FBFF880BDBC4F2FCBD6B32C8953FE603D43B23DADDF398783EB3F30D99FF227934D96FAE9EABB8D792EA47D4663B68D80944825B7654F7110D2E173A54920DA1D79EB86515C5904AE767BA11A623D402E6B8B4E6B060271EA8A2BDCA27AF23764AC10D2340196979CC65B3AC87D62B749033F7008129B4D92445C8EE7844846A5ABC03BEEFDFA331E83722A59F0089E4EF6D480121349A5468B796EC6176B93E2B993D9AB129C73B2162C5AC2CC0A16C13A4EB450C4AE5E4B5062AC4BE6809235C6045F9A225989A3F6499C8E9863882528969A01AD4C26C423AD2C180D28F2FD8E253C403B380D24C0CD3009172E61AC70313731391551B5544FCD28D5F2B0088CB1856674B3C06A3CC1ECA919254C5DA9A2880C86CCDA69C6123E192BB4394A658D31C9D7DC5459B14BFD57E533894CA2312D204D074B13281E19362CC3A231244CE10063C16CF2B23659F6089156F6DC949AA80ED9E7FAD4AA1C102CADEAA93EA57A9207965ABD6434C064CD6296C0C9F8AB980354D6580E8639AAB4B06FC55514172845EC23DEC1A6B1BC0AB4348780B790F95CEA10C16680090BCE8A1F0D347E2AF890535CA214B1AF6F5514572845ECB3C8C1305C5E875AC270E1A3660E5CB425BA33A98226D776245818660D5AE2BBAD56604C4FDC98D70A8CE931818FA12157A5DDEE1507C26A795B6D4BDFE6CE900D942CD612933213819895321AD358839688D55AC150975EC0F727008FA33C4BE6F7FF96D09579CA9A630B6987BEB38B58BDB537361CF9574947C414F3D890168600E5E721DDCF7FDD08011D1A39774483F321D7527FAB9DD858C0DD04E8C528CACCF828798B9F28D3AF490A52CDF844BF1CB1BC5BB4BB57940E9C9E26776EFA89D14D94448A2EA3441BC896B78059C0E7A2313E17D6F12989CDD41C9F4288010B0095C4B3FAFF02A818BFC11242197737137832CDCC8FB6F8D40BDE726354A0822B9E2693E6E7F693D1738CBB601314011ACE068AC6ADE6045FC6D38151E7DA68D90C47CB6E70248908D71C470BDB389284D01B2B8E0419D8C6D1AA198E56DDE04812A0AD398E96B67124896837561C093250E3487064E3AB94A7DBFC49F9BB7464CB9DC86ADE6DA990125FB5543851EED0C67B956555A6132A836FEE2EF5287B8A62B24F0179B6F98747314ACFD35585778EEFDE9328CE320D4D17E7F3C431DF739D28F32ECCFDE52EF8CFD7B51CE8E6CBC4818EECF633BEB9B91B5E42258A76B5444862C626D1FD4C2F5512EC74A6CE9454B4CBBE907613C92A53211966E5AAFCD8B24EFC6F4EB8FDEA847FD83B8F7F64A9354B1596F2DC8025F60687A1248439B8A19AF1F172FACFB4E9C5E4E6EFB7F5D6AF261F428AC38BC9F9E45F0A46B4F376613E637A58401CC5D460281B768806D1E9CC122A4AA733637ADAD302C440D39A10D86D4B3D1F45BB0EA7A3F204B3300D821798259A6CDECD26CB9D77FE325BEEF5D65D2C77CCE5CA24F3BC7EE279D432A69B75BE911499A6062284E7B2F134B6EF1E7017EB4EDF407E572698683EC51D6B1DD67DCB828E60FDB72C90AB5CB82C10AB7B70758715ECDEDB042F687A0CB3B404BA209BB75024F3F64BB93E84264C2C6C31710BAE3A6D3ECAE6182BFA8B88BDC86AC5CA5C9F17FD24D08A9B06CDACB2D8F15B23AF6CD5B473EDF8B23A782FB6264C2C6D31B16AC1C4EA454FD8D0134A5E162D795958E465D992976507FA13F450D4539AA853A25A69324D3B549A353F470BFBB69A9F63F3A3A7E8D998D1BA739B1D637B3BFAE8BF942157423D50A1DE836A50314D3B0455CD217134B797800BA2191E04025DE0027003D4DC9FC1BE7F1ABBB3A261878860DC092DE0417025349B47AEB9855914C350CB429C325115D800BCFD478AAEC74855E68910A35F370A352DFD2C5CFB5A7E62125F1AF9A613BEE4EB399CB4889DEA5E539E3E446BD23A020C73F7DA34554E23F018CDA4B5143427911144154C5798BDBE331141B1661B67386F91D6A8BF2446D2AF338DAF134796B5FE05442306D189659C6F9F73EE050D923EA51ECB8300C2825A790184054080CEC743E6181EEFA6054AEAA1C8F9F12C00A6159E60949B19DDE0E62F203B6D900DBAD9D18D09FF02B2D306191614645890A9B23BBF80ECA44086C589E973978EE422E8608BFE0291061019C9590EC9D5F0829291A164A8031E07130B9BA11798740813F0E3D1FE6162613BF302930E61027E1BDAAF491412DC88B6BD431946FBC693A17134AB3EE4EE16CF233312937A99E697E9BE7C76F2C6743C40E4F82CE9EAAC3B2374E129533CB36FB3E2D97371E4C14337A26E9283BBF3C8B3158D104979AE6F8685FCC973411116A21171AA1C1C41B24C4F23C44F1A8C81E5207BF05CD083644882834BF48C1D2E0449F931259F848A9FBA3C2A0DC37891DA4B98C62CD4C8E5747797EC94325759387F98409745A2409D2D84FA90A517E33BCA766D4217D963883892D88FA7CBEEE205EA6C21D44755AEDD13DA8BAC073571D60742E8802D843A91E5F6E63B824ED342875025A863B19E9A8172FF2CF45A96405D61C922C501161B2C71544509381424FFA2403F7FF10AD4F3E7106D389B219C6490F35207920CCAFCD8D1970F13E58A7D2CE83FFEDCC5B4CB1EF02F1DE32C8ACCBD01965F94AFD29249F4328069C73E6E3D44C0ED1718A5CA3918BC431158960E56D42A7C64D90E060BA6935639B19EC86021374A3C6132EE6D79B2C395E50AC77D094F64B8120F382811B0A6BF9C85C1E32F6D2C6E6187C20021A0EBD7F5DC8401668DD7F53F7A6EC20093C7EBFAC99CA830040F0D5C3F225E1CCF63E0B21703E298F03C060E6A00B9A9FD790C1C5CED72E3F1490F9C3575CA0F2DB0E1AFF5D0FB38BAD44C72C02871935DCB7319772DB061F39059786541E623E865A53433593C617337151B369D55FB210B360E60B8723B88C5A1D62E4D36556EA5F6C3E42EE28141CAAEEA2D0E91BDA14DDB650FB4072804AB2ECBD6B3EC52297F407F0A41A9D7B34F47DA7A9F8540585F93C87DA848ACABD07D15D1A24E12E6B4B828E7382AAAF0513B48ECEC9CD8791DC6EEBDB38D69F136D1B5FEC374F2ABE31D699537FB3BB2BBF13F1CE3C3314E64B7BFF36AB9F0928B7659FFEB99C0F3FA431A6637B23104CAA64B87403EF83F1E5D6F57F2FD1608308190486EF0F3701CC95CC649588E87A792D2FB8087374628175F6978F84CF6078F128B3EF81BE71B69C2DB9788FC421E9CED53115B1C27A29E88BAD8D7D7AE43B5F23ECA6954EDE94F8AE1DDFEF1FBFF01C74BAF728DEE0000 , N'6.2.0-61023')
END
