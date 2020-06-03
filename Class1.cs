using System;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;
using System.Collections.Generic;
using NativeUI;


namespace GTA5JetPack
{
    public class JetPack : Script   //继承“Script”类
    {
        GTA.Timer StopFire = new GTA.Timer();
        Ped player;//设player是主角
        Vector3 PlayerPos;//Player位置
        Ped FriendlyJetPackDriver = null;//声明FriendlyJetPackDriver1全局变量
        int FriendlyHp = 20000;//友军hp
        int FriendlyArmor = 200;//友军护甲
        int EnemyHp = 20000;//敌军hp
        int EnemyArmor = 200;//敌军护甲
        Vehicle FriendlyJetPack_rocket = null;//声明FriendlyJetPack全局变量
        Vehicle FriendlyJetPack_minigun = null;//声明FriendlyJetPack全局变量
        Ped EnemyJetPackDriver = null;//声明EnemyJetPackDriver1全局变量
        Vehicle EnemyJetPack_rocket = null;//声明EnemyJetPack全局变量
        Vehicle EnemyJetPack_minigun = null;//声明EnemyJetPack全局变量
        Blip Friendlyblip;
        Blip Enemyblip;
        float nX;
        float nY;
        int FireTime = 60000;//尸体燃烧时间

        List<Tuple<Ped, Vehicle, Blip>> FriendlyJet = new List<Tuple<Ped, Vehicle, Blip>>();//(创建列表以记录刷出的友军人物和载具)
        List<Tuple<Ped, Vehicle, Blip>> EnemyJet = new List<Tuple<Ped, Vehicle, Blip>>();//(创建列表以记录刷出的友军人物和载具)
        List<Tuple<Ped, GTA.Timer>> StopFireTimer = new List<Tuple<Ped, GTA.Timer>> ();//(创建列表以记录被炸着火的NPC及灭火执行时间)
        //读取ini文件及设置string全局变量
        ScriptSettings config;
        Keys OpenMenu;
        string FriendlyPedModelName1;//1号友军模型名
        string FriendlyPedModelName2;//2号友军模型名
        string FriendlyPedModelName3;//3号友军模型名
        string FriendlyPedModelName;//友军模型名

        string FriendlyVehicleModelName;//友军载具模型名

        string EnemyPedModelName1;//1号敌军模型名
        string EnemyPedModelName2;//2号敌军模型名
        string EnemyPedModelName3;//3号敌军模型名
        string EnemyPedModelName;//敌军模型名

        string EnemyVehicleModelName;//敌军载具模型名
        //自动选择语言模块全局变量
        int L;//LanguageID
        int FriendlyPed_List;
        int EnemyPed_List;
        string[] title1 = { "JetPack", "JetPack", "JetPack", "JetPack", "JetPack", "JetPack", "JetPack", "реактивный пакет", "JetPack", "火箭飛行兵", "ジェットパック", "JetPack", "火箭飞行兵" };
        string[] title2 = { "Menu", "Menu", "Menü", "Menu", "Menú", "Menu", "Menu", "меню", "Menu", "菜單", "メニュー", "Menú", "菜单" };
        string[] menu1 = { "GiveJetPack", "DonnerJetPack", "GebenJetPack", "DaiJetPack", "DarJetPack", "Dê JetPack", "Dodaj JetPack", "давать реактивный пакет", "JetPack주다", "給噴射背包", "ジェットパックを作成", "DarJetPack", "刷出喷气背包" };
        string[] menu2 = { "SpawnFriendlyJetPack-Rocket", "SpawnAmicalJetPack", "GenerierenFreundlichJetPack", "GeneraFriendlyJetPack", "CrearAmigableJetPack", "CriarAmigávelJetPack", "UtwórzPrzyjaznyJetPack", "Создать дружелюбный", "우호적JetPack생성", "創建友軍飛行兵", "友軍を作成", "CrearAmigableJetPack", "刷带导弹的友军火箭飞行兵" };
        string[] menu3 = { "SpawnFriendlyJetPack-Minigun", "SpawnAmicalJetPack", "GenerierenFreundlichJetPack", "GeneraFriendlyJetPack", "CrearAmigableJetPack", "CriarAmigávelJetPack", "UtwórzPrzyjaznyJetPack", "Создать дружелюбный", "우호적JetPack생성", "創建友軍飛行兵", "友軍を作成", "CrearAmigableJetPack", "刷带机枪友军火箭飞行兵" };
        string[] menu4 = { "SpawnEnemyJetPack-Rocket", "SpawnEnnemiJetPack", "GenerierenFeindJetPack", "GeneraNemicoJetPack", "CrearEnemigoJetPack", "CriarInimigoJetPack", "UtwórzWrógJetPack", "Создать враг", "적JetPack생성", "創建敵軍飛行兵", "敵を作成", "CrearEnemigoJetPack", "刷带导弹的敌军火箭飞行兵" };
        string[] menu5 = { "SpawnEnemyJetPack-Minigun", "SpawnEnnemiJetPack", "GenerierenFeindJetPack", "GeneraNemicoJetPack", "CrearEnemigoJetPack", "CriarInimigoJetPack", "UtwórzWrógJetPack", "Создать враг", "적JetPack생성", "創建敵軍飛行兵", "敵を作成", "CrearEnemigoJetPack", "刷带机枪敌军火箭飞行兵" };
        string[] menu6 = { "KillFriendlyJetPack", "TuerAmicalJetPack", "TötenFreundlichJetPack", "UccidiFriendlyJetPack", "MatarAmigableJetPack", "MatarAmigávelJetPack", "ZabijPrzyjaznyJetPack", "убить дружелюбный", "우호적JetPack죽이다", "殺死友軍飛行兵", "友軍を殺す", "MatarAmigableJetPack", "杀死友军火箭飞行兵" };
        string[] menu7 = { "KillEnemyJetPack", "TuerEnnemiJetPack", "TötenFeindJetPack", "UccidiNemicoJetPack", "MatarEnemigoJetPack", "MatarInimigoJetPack", "ZabijWrógJetPack", "убить враг", "적JetPack죽이다", "殺死敵軍飛行兵", "敵を殺す", "MatarEnemigoJetPack", "杀死敌军火箭飞行兵" };
        string[] Spawed1 = { "JetPack Spawed!!!", "JetPack Spawed!!!", "JetPackGeneriert!!!", "GeneratoJetPack!!!", "CrearJetPackCompletar!!!", "JetPackCriado!!!", "StworzoneJetPack!!!", "Создание завершено！！！", "JetPack생성완성", "已創建噴射背包！！！", "ジェットパックを作成した！！！", "CrearJetPackCompletar!", "喷气背包已刷出！！！" };
        string[] Spawed2 = { "FriendlyJetPack Spawed!!!", "AmicalJetPack Spawed!!!", "FreundlichJetPackGeneriert!!!", "GeneratoFriendlyJetPack!!!", "CrearAmigableCompletar!!!", "AmigávelJetPackCriado!!!", "StworzonePrzyjaznyJetPack!!!", "дружелюбный Создание завершено！！！", "우호적JetPack생성완성", "已創建友軍飛行兵！！！", "友軍を作成した！！！", "CrearAmigableCompletar!", "已刷出友军火箭飞行兵！！！" };
        string[] Spawed3 = { "EnemyJetPack Spawed!!!", "EnnemiJetPack Spawed!!!", "JetPackGeneriert!!!", "GeneratoNemicoJetPack!!!", "CrearrEnemigoCompletar!!!", "InimigoJetPackCriado!!!", "StworzoneWrógJetPack!!!", "враг Создание завершено！！！", "적JetPack생성완성", "已創建敵軍飛行兵!!!", "敵を作成した！！！", "CrearrEnemigoCompletar!", "已刷出敌军火箭飞行兵！！！" };
        //GTA NativeUI菜单
        MenuPool modMenuPool;
        UIMenu mainMenu;
        UIMenuItem giveJetPack;
        UIMenuItem spawnFriendlyJetPack_rocket;
        UIMenuItem spawnFriendlyJetPack_minigun;
        UIMenuItem spawnEnemyJetPack_rocket;
        UIMenuItem spawnEnemyJetPack_minigun;
        UIMenuItem killFriendlyJetPack;
        UIMenuItem killEnemyJetPack;
        public JetPack()
        {
            //自动选择语言
            L = Function.Call<int>(Hash._GET_UI_LANGUAGE_ID);
            //读取配置文件部分
            config = ScriptSettings.Load("scripts\\JetPackSettings.ini");//读取配置文件
            OpenMenu = config.GetValue<Keys>("Options", "OpenMenu", Keys.F5); //读取热键内容
            FriendlyPedModelName1 = config.GetValue<string>("Peds1", "FriendlyPedModelName1", "s_m_m_highsec_01");//读取友军人物模型1名称
            FriendlyPedModelName2 = config.GetValue<string>("Peds1", "FriendlyPedModelName2", "s_m_m_highsec_01");//读取友军人物模型2名称
            FriendlyPedModelName3 = config.GetValue<string>("Peds1", "FriendlyPedModelName3", "s_m_m_highsec_01");//读取友军人物模型3名称
            FriendlyVehicleModelName = config.GetValue<string>("Peds1", "FriendlyVehicleModelName", "thruster‬");//读取友军人物驾驶的载具模型名称

            EnemyPedModelName1 = config.GetValue<string>("Peds2", "EnemyPedModelName1", "s_m_m_chemsec_01‬");//读取敌军人物模型1名称
            EnemyPedModelName2 = config.GetValue<string>("Peds2", "EnemyPedModelName2", "s_m_m_chemsec_01‬");//读取敌军人物模型2名称
            EnemyPedModelName3 = config.GetValue<string>("Peds2", "EnemyPedModelName3", "s_m_m_chemsec_01‬");//读取敌军人物模型3名称
            EnemyVehicleModelName = config.GetValue<string>("Peds2", "EnemyVehicleModelName", "thruster‬");//读取敌军人物驾驶的载具模型名称
            //FireTime = config.GetValue<int>("Settings", "BodyFireTime", 10000);
            //创建NativeUI菜单及菜单选项
            modMenuPool = new MenuPool();
            mainMenu = new UIMenu(title1[L], title2[L]);
            modMenuPool.Add(mainMenu);

            giveJetPack = new UIMenuItem(menu1[L]);
            mainMenu.AddItem(giveJetPack);

            spawnFriendlyJetPack_rocket = new UIMenuItem(menu2[L]);
            mainMenu.AddItem(spawnFriendlyJetPack_rocket);

            spawnFriendlyJetPack_minigun = new UIMenuItem(menu3[L]);
            mainMenu.AddItem(spawnFriendlyJetPack_minigun);

            spawnEnemyJetPack_rocket = new UIMenuItem(menu4[L]);
            mainMenu.AddItem(spawnEnemyJetPack_rocket);

            spawnEnemyJetPack_minigun = new UIMenuItem(menu5[L]);
            mainMenu.AddItem(spawnEnemyJetPack_minigun);

            killFriendlyJetPack = new UIMenuItem(menu6[L]);
            mainMenu.AddItem(killFriendlyJetPack);

            killEnemyJetPack = new UIMenuItem(menu7[L]);
            mainMenu.AddItem(killEnemyJetPack);

            mainMenu.OnItemSelect += onMainMenuItemSelect;

            KeyDown += OnKeyDown;//按键按下事件
            Tick += OnTick;
            Tick += OnTick2;
            Tick += OnTick3;
        }
        void onMainMenuItemSelect(UIMenu sender, UIMenuItem item, int index)
        {
            if (item == giveJetPack)
            {
                GiveJetPack();
            }
            else if (item == spawnFriendlyJetPack_rocket)
            {
                SpawnFriendlyJetPack_rocket();
            }
            else if (item == spawnFriendlyJetPack_minigun)
            {
                SpawnFriendlyJetPack_minigun();
            }
            else if (item == spawnEnemyJetPack_rocket)
            {
                SpawnEnemyJetPack_rocket();
            }
            else if (item == spawnEnemyJetPack_minigun)
            {
                SpawnEnemyJetPack_minigun();
            }
            else if (item == killFriendlyJetPack)
            {
                KillFriendlyJetPack();
            }
            else if (item == killEnemyJetPack)
            {
                KillEnemyJetPack();
            }
        }
        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == OpenMenu && !modMenuPool.IsAnyMenuOpen())
            {
                mainMenu.Visible = !mainMenu.Visible;
            } else if (e.KeyCode == OpenMenu && modMenuPool.IsAnyMenuOpen())
            {
                mainMenu.Visible = !mainMenu.Visible;
            }
        }
        void OnTick(object sender, EventArgs e)
        {
            player = Game.Player.Character;
            PlayerPos = Game.Player.Character.Position;
            bool isExist = Function.Call<bool>(Hash.DOES_ENTITY_EXIST, player);//人物是否存在，如果不存在有可能是在加载游戏
            if (isExist)
            {
                if (FriendlyJetPack_rocket == null && FriendlyJetPack_minigun == null && EnemyJetPack_rocket == null && EnemyJetPack_minigun == null)
                {

                }
                else if (EnemyJetPack_rocket == null && EnemyJetPack_minigun == null && EnemyJetPackDriver == null)
                {
                    AttackEnemy();
                }
                else if (FriendlyJetPack_rocket == null && FriendlyJetPack_minigun == null && FriendlyJetPackDriver == null)
                {
                    AttackFriendly();
                }
                else
                {
                    AttackEnemy();
                    AttackFriendly();
                }
            }
            else
            {
                return;
            }
        }
        void OnTick2(object sender, EventArgs e)
        {
            if (modMenuPool != null)
                modMenuPool.ProcessMenus();
            Random random = new Random();
            Random List1 = new Random();
            Random List2 = new Random();
            FriendlyPed_List = List1.Next(0, 3);//生成FriendlyPed序号

            EnemyPed_List = List2.Next(0, 3);//生成EnemyPed序号

            nX = random.Next(-10, 11);   //生成-10-10且不能是-5-5之间的随机数
            if (nX > -5 || nX < 5)
            {
                random.Next(-10, 11);
            }
            nY = random.Next(-10, 11);   //生成-10-10且不能是-5-5之间的随机数
            if (nY > -5 || nY < 5)
            {
                random.Next(-10, 11);
            }
        }
        void OnTick3(object sender, EventArgs e)
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in FriendlyJet)
            {
                /**if (tuple.Item1.IsDead || tuple.Item2.IsDead)
                {
                    FireFriendly();
                }**/
                
                Vector3 FriendlyJetPackPos = tuple.Item1.Position;//获取友军坐标
                float DistanceA = FriendlyJetPackPos.X - PlayerPos.X;
                float DistanceB = FriendlyJetPackPos.Y - PlayerPos.Y;
                if (DistanceA > 500f || DistanceB > 500f)
                {
                    FriendlyJetPackGotoPlayer();
                }
            }
            FireFriendly();
            FireEnemy();
            FireStop();
            /**foreach (Tuple<Ped, Vehicle, Blip> tuple in EnemyJet)
            {
                if (tuple.Item1.IsDead || tuple.Item2.IsDead)
                {
                    FireEnemy();
                }
            }**/
        }
        void GiveJetPack()
        {
            Vector3 JetPackPos = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 2f;//喷气背包刷出位置
            Vehicle JetPack = World.CreateVehicle(new Model(VehicleHash.Thruster), JetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, JetPack, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, JetPack, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, JetPack, 10, 1, true);//给予喷气背包导弹的改装件
            Function.Call(Hash.SET_PED_INTO_VEHICLE, player, JetPack, -1);//主角进入喷气背包驾驶位
            UI.Notify(Spawed1[L]);
        }
        void SpawnFriendlyJetPack_rocket()
        {
            float n1 = -25f + nX;
            float n2 = -26f + nX;
            if (FriendlyPed_List == 0)
            {
                FriendlyPedModelName = FriendlyPedModelName1;
            }
            else if (FriendlyPed_List == 1)
            {
                FriendlyPedModelName = FriendlyPedModelName2;
            }
            else
            {
                FriendlyPedModelName = FriendlyPedModelName3;
            }
            Vector3 FriendlyJetPackPos = Game.Player.Character.Position + Game.Player.Character.UpVector * 40f + Game.Player.Character.ForwardVector * n1;//喷气背包刷出位置
            FriendlyJetPack_rocket = World.CreateVehicle(FriendlyVehicleModelName, FriendlyJetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, FriendlyJetPack_rocket, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, FriendlyJetPack_rocket, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, FriendlyJetPack_rocket, 10, 1, true);//给予喷气背包导弹的改装件

            Vector3 FriendlyJetPackDriverPos = Game.Player.Character.Position + Game.Player.Character.UpVector * 40f + Game.Player.Character.ForwardVector * n2;//喷气背包驾驶员JetPackDriver刷出位置
            FriendlyJetPackDriver = World.CreatePed(FriendlyPedModelName, FriendlyJetPackDriverPos);//喷气背包驾驶员JetPackDriver刷出
            Function.Call(Hash.SET_PED_INTO_VEHICLE, FriendlyJetPackDriver, FriendlyJetPack_rocket, -1);//JetPackDriver进入喷气背包驾驶位
            FriendlyJetPackDriver.Task.FightAgainstHatedTargets(500f);//对抗半径500米的一切敌对目标
            FriendlyJetPackDriver.AlwaysKeepTask = true;//始终保持任务
            FriendlyJetPackDriver.Health = FriendlyHp;//生命值
            FriendlyJetPackDriver.Armor = FriendlyArmor;//护甲
            FriendlyJetPackDriver.RelationshipGroup = Game.Player.Character.RelationshipGroup;//设置喷气背包驾驶员JetPackDriver属于主角阵营
            Friendlyblip = FriendlyJetPack_rocket.AddBlip();
            Friendlyblip.Sprite = (BlipSprite)597;//强制转换枚举
            FriendlyJetPack_rocket.CurrentBlip.Color = BlipColor.Blue;
            FriendlyJetPack_rocket.CurrentBlip.Scale = 0.6f;

            FriendlyJet.Add(new Tuple<Ped, Vehicle, Blip>(FriendlyJetPackDriver, FriendlyJetPack_rocket, Friendlyblip));

            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, FriendlyJetPackDriver, FriendlyJetPack_rocket, PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 30f, 1f, 1489874736, 16777216, 1f, true);
            UI.Notify(Spawed2[L]);
        }
        void SpawnFriendlyJetPack_minigun()
        {
            float n1 = -25f + nX;
            float n2 = -26f + nX;
            if (FriendlyPed_List == 0)
            {
                FriendlyPedModelName = FriendlyPedModelName1;
            }
            else if (FriendlyPed_List == 1)
            {
                FriendlyPedModelName = FriendlyPedModelName2;
            }
            else
            {
                FriendlyPedModelName = FriendlyPedModelName3;
            }
            Vector3 FriendlyJetPackPos = Game.Player.Character.Position + Game.Player.Character.UpVector * 40f + Game.Player.Character.ForwardVector * n1;//喷气背包刷出位置
            FriendlyJetPack_minigun = World.CreateVehicle(FriendlyVehicleModelName, FriendlyJetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, FriendlyJetPack_minigun, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, FriendlyJetPack_minigun, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, FriendlyJetPack_minigun, 10, 0, true);//给予喷气背包火神机枪的改装件

            Vector3 FriendlyJetPackDriverPos = Game.Player.Character.Position + Game.Player.Character.UpVector * 40f + Game.Player.Character.ForwardVector * n2;//喷气背包驾驶员JetPackDriver刷出位置
            FriendlyJetPackDriver = World.CreatePed(FriendlyPedModelName, FriendlyJetPackDriverPos);//喷气背包驾驶员JetPackDriver刷出
            Function.Call(Hash.SET_PED_INTO_VEHICLE, FriendlyJetPackDriver, FriendlyJetPack_minigun, -1);//JetPackDriver进入喷气背包驾驶位
            FriendlyJetPackDriver.Task.FightAgainstHatedTargets(500f);//对抗半径500米的一切敌对目标
            FriendlyJetPackDriver.AlwaysKeepTask = true;//始终保持任务
            FriendlyJetPackDriver.Health = FriendlyHp;//生命值
            FriendlyJetPackDriver.Armor = FriendlyArmor;//护甲
            FriendlyJetPackDriver.RelationshipGroup = Game.Player.Character.RelationshipGroup;//设置喷气背包驾驶员JetPackDriver属于主角阵营
            Friendlyblip = FriendlyJetPack_minigun.AddBlip();
            Friendlyblip.Sprite = (BlipSprite)597;//强制转换枚举
            FriendlyJetPack_minigun.CurrentBlip.Color = BlipColor.Blue;
            FriendlyJetPack_minigun.CurrentBlip.Scale = 0.6f;

            FriendlyJet.Add(new Tuple<Ped, Vehicle, Blip>(FriendlyJetPackDriver, FriendlyJetPack_minigun, Friendlyblip));

            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, FriendlyJetPackDriver, FriendlyJetPack_minigun, PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 30f, 1f, 1489874736, 16777216, 1f, true);
            UI.Notify(Spawed2[L]);
        }
        void FriendlyJetPackGotoPlayer()
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in FriendlyJet)
            {
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, tuple.Item1, tuple.Item2, PlayerPos.X + nX, PlayerPos.Y + nY, PlayerPos.Z + 5f, 160f, 6f, 1489874736, 16777216, 1f, true);
            }
        }
        void AttackEnemy()
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in FriendlyJet)
            {
                Ped[] Peds = World.GetNearbyPeds(player, 150f);//收集半径60米的一切NPC（包括动物）Ped数组为Peds
                foreach (Ped p in Peds)//阅历数组Peds中是否有符合Ped p的NPC
                {
                    bool IsShoot = Function.Call<bool>(Hash.IS_PED_SHOOTING, p);
                    bool IsCombat = Function.Call<bool>(Hash.IS_PED_IN_COMBAT, p, player);
                    if (p.IsAlive && p.IsHuman && IsPedEnemyOrNeutral(p) || IsShoot == true || IsCombat == true)//数组中是否有是人类的活着的敌对目标
                    {
                        Vector3 shootTo = p.Position;//获取敌对目标坐标
                        Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, tuple.Item1, tuple.Item2, shootTo.X, shootTo.Y, shootTo.Z, 180f, 10f, 1489874736, 16777216, 1f, true);//驶向目标
                        Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, tuple.Item1, p, shootTo.X, shootTo.Y, shootTo.Z);//使用载具武器射击目标
                    }
                    else if (IsPedFriendly(p) || p.IsPlayer || (player.IsInVehicle() && p.IsInVehicle(player.CurrentVehicle)))
                    {
                        FriendlyJetPackGotoPlayer();
                    }
                }
            }
        }
        void KillFriendlyJetPack()
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in FriendlyJet)
            {
                Function.Call(Hash.TASK_LEAVE_VEHICLE, tuple.Item1, tuple.Item2, 4160);
                Function.Call(Hash.START_ENTITY_FIRE, tuple.Item1);
                this.StopFire.Set(FireTime);
                StopFireTimer.Add(new Tuple<Ped, GTA.Timer>(tuple.Item1, this.StopFire));
                //tuple.Item1.Kill();
                tuple.Item2.Explode();
                FriendlyJet.RemoveAll(t => t.Item1.IsDead);
                FriendlyJet.RemoveAll(t => t.Item2.IsDead);
                tuple.Item3.Remove();
                return;
            }
        }
        void SpawnEnemyJetPack_rocket()
        {
            float n1 = 100f + nX;
            float n2 = 40f + nY;
            float n3 = 101f + nX;
            float n4 = 40f + nY;
            if (EnemyPed_List == 0)
            {
                EnemyPedModelName = EnemyPedModelName1;
            }
            else if (EnemyPed_List == 1)
            {
                EnemyPedModelName = EnemyPedModelName2;
            }
            else
            {
                EnemyPedModelName = EnemyPedModelName3;
            }
            Vector3 EnemyJetPackPos = Game.Player.Character.Position + Game.Player.Character.UpVector * n2 + Game.Player.Character.ForwardVector * n1;//喷气背包刷出位置
            EnemyJetPack_rocket = World.CreateVehicle(EnemyVehicleModelName, EnemyJetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, EnemyJetPack_rocket, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, EnemyJetPack_rocket, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, EnemyJetPack_rocket, 10, 1, true);//给予喷气背包导弹的改装件

            Vector3 EnemyJetPackDriverPos = Game.Player.Character.Position + Game.Player.Character.UpVector * n4 + Game.Player.Character.ForwardVector * n3;//喷气背包驾驶员JetPackDriver刷出位置
            EnemyJetPackDriver = World.CreatePed(EnemyPedModelName, EnemyJetPackDriverPos);//喷气背包驾驶员JetPackDriver刷出
            Function.Call(Hash.SET_PED_INTO_VEHICLE, EnemyJetPackDriver, EnemyJetPack_rocket, -1);//JetPackDriver进入喷气背包驾驶位

            EnemyJetPackDriver.AlwaysKeepTask = true;//始终保持任务
            EnemyJetPackDriver.Health = EnemyHp;//生命值
            EnemyJetPackDriver.Armor = EnemyArmor;//护甲
            int EnemyRelationShipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER"); //创建恨主角的阵营指数
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, EnemyJetPackDriver, EnemyRelationShipGroup);//设置阵营为敌对阵营
            Enemyblip = EnemyJetPack_rocket.AddBlip();
            Enemyblip.Sprite = (BlipSprite)597;//强制转换枚举
            EnemyJetPack_rocket.CurrentBlip.Color = BlipColor.Red;
            EnemyJetPack_rocket.CurrentBlip.Scale = 0.6f;

            EnemyJet.Add(new Tuple<Ped, Vehicle, Blip>(EnemyJetPackDriver, EnemyJetPack_rocket, Enemyblip));
            UI.Notify(Spawed3[L]);
        }
        void SpawnEnemyJetPack_minigun()
        {
            float n1 = 100f + nX;
            float n2 = 40f + nY;
            float n3 = 101f + nX;
            float n4 = 40f + nY;
            if (EnemyPed_List == 0)
            {
                EnemyPedModelName = EnemyPedModelName1;
            }
            else if (EnemyPed_List == 1)
            {
                EnemyPedModelName = EnemyPedModelName2;
            }
            else
            {
                EnemyPedModelName = EnemyPedModelName3;
            }
            Vector3 EnemyJetPackPos = Game.Player.Character.Position + Game.Player.Character.UpVector * n2 + Game.Player.Character.ForwardVector * n1;//喷气背包刷出位置
            EnemyJetPack_minigun = World.CreateVehicle(EnemyVehicleModelName, EnemyJetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, EnemyJetPack_minigun, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, EnemyJetPack_minigun, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, EnemyJetPack_minigun, 10, 0, true);//给予喷气背包火神机枪的改装件

            Vector3 EnemyJetPackDriverPos = Game.Player.Character.Position + Game.Player.Character.UpVector * n4 + Game.Player.Character.ForwardVector * n3;//喷气背包驾驶员JetPackDriver刷出位置
            EnemyJetPackDriver = World.CreatePed(EnemyPedModelName, EnemyJetPackDriverPos);//喷气背包驾驶员JetPackDriver刷出
            Function.Call(Hash.SET_PED_INTO_VEHICLE, EnemyJetPackDriver, EnemyJetPack_minigun, -1);//JetPackDriver进入喷气背包驾驶位

            EnemyJetPackDriver.AlwaysKeepTask = true;//始终保持任务
            EnemyJetPackDriver.Health = EnemyHp;//生命值
            EnemyJetPackDriver.Armor = EnemyArmor;//护甲
            int EnemyRelationShipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER"); //创建恨主角的阵营指数
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, EnemyJetPackDriver, EnemyRelationShipGroup);//设置阵营为敌对阵营
            Enemyblip = EnemyJetPack_minigun.AddBlip();
            Enemyblip.Sprite = (BlipSprite)597;//强制转换枚举
            EnemyJetPack_minigun.CurrentBlip.Color = BlipColor.Red;
            EnemyJetPack_minigun.CurrentBlip.Scale = 0.6f;

            EnemyJet.Add(new Tuple<Ped, Vehicle, Blip>(EnemyJetPackDriver, EnemyJetPack_minigun, Enemyblip));
            UI.Notify(Spawed3[L]);
        }
        void EnemyJetPackGotoPlayer()
        {
            Random random = new Random();
            float n = random.Next(-10, 11);   //生成-10-10且不能是-5-5之间的随机数
            if (n > -5 || n < 5)
            {
                random.Next(-10, 11);
            }
            foreach (Tuple<Ped, Vehicle, Blip> tuple in EnemyJet)
            {
                Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, tuple.Item1, tuple.Item2, PlayerPos.X + n, PlayerPos.Y + n, PlayerPos.Z, 60f, 6f, 1489874736, 16777216, 1f, true);
                Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, tuple.Item1, player, PlayerPos.X, PlayerPos.Y, PlayerPos.Z);//使用载具武器射击玩家
            }
        }
        void AttackFriendly()
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in EnemyJet)
            {
                Ped[] Peds = World.GetNearbyPeds(player, 150f);//收集半径60米的一切NPC（包括动物）Ped数组为Peds

                foreach (Ped p in Peds)//阅历数组Peds中是否有符合Ped p的NPC
                {
                    if (p.IsAlive && p.IsHuman && IsPedFriendly(p) && p.IsPlayer == false)//数组中是否有是人类的活着的友军目标
                    {
                        Vector3 shootTo = p.Position;//获取友军目标坐标
                        Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, tuple.Item1, tuple.Item2, shootTo.X + nX, shootTo.Y + nY, shootTo.Z, 80f, 20f, 1489874736, 16777216, 1f, true);//驶向目标
                        Function.Call(Hash.SET_VEHICLE_SHOOT_AT_TARGET, tuple.Item1, p, shootTo.X, shootTo.Y, shootTo.Z);//使用载具武器射击目标
                    }
                    else
                    {
                        EnemyJetPackGotoPlayer();
                    }
                }
            }
        }
        void KillEnemyJetPack()
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in EnemyJet)
            {
                Function.Call(Hash.TASK_LEAVE_VEHICLE, tuple.Item1, tuple.Item2, 4160);
                Function.Call(Hash.START_ENTITY_FIRE, tuple.Item1);
                this.StopFire.Set(FireTime);
                StopFireTimer.Add(new Tuple<Ped, GTA.Timer>(tuple.Item1, this.StopFire));
                //tuple.Item1.Kill();
                tuple.Item2.Explode();
                EnemyJet.RemoveAll(t => t.Item1.IsDead);
                EnemyJet.RemoveAll(t => t.Item2.IsDead);
                tuple.Item3.Remove();
                return;
            }
        }
        bool IsPedEnemyOrNeutral(Ped p) //判断是否为敌人
        {
            Relationship rel = p.GetRelationshipWithPed(Game.Player.Character);
            return (rel == Relationship.Hate || rel == Relationship.Dislike || rel == Relationship.Neutral);
        }
        bool IsPedFriendly(Ped p)//判断是否为朋友
        {
            Relationship rel = p.GetRelationshipWithPed(Game.Player.Character);
            return (rel == Relationship.Like || rel == Relationship.Companion || rel == Relationship.Respect);
        }
        void FireFriendly()
        {
            foreach (Tuple<Ped, Vehicle, Blip> tuple in FriendlyJet)
            {
                if (tuple.Item1.IsDead || tuple.Item2.IsDead)
                {
                    Function.Call(Hash.TASK_LEAVE_VEHICLE, tuple.Item1, tuple.Item2, 4160);
                    Function.Call(Hash.START_ENTITY_FIRE, tuple.Item1);
                    this.StopFire.Set(FireTime);
                    StopFireTimer.Add(new Tuple<Ped, GTA.Timer>(tuple.Item1, this.StopFire));
                    tuple.Item2.Explode();
                    FriendlyJet.RemoveAll(t => t.Item1.IsDead);
                    FriendlyJet.RemoveAll(t => t.Item2.IsDead);
                    tuple.Item3.Remove();
                    return;
                }
            }
        }
        void FireEnemy()
        {
            
            foreach (Tuple<Ped, Vehicle, Blip> tuple in EnemyJet)
            {
                if (tuple.Item1.IsDead || tuple.Item2.IsDead)
                {
                    Function.Call(Hash.TASK_LEAVE_VEHICLE, tuple.Item1, tuple.Item2, 4160);
                    Function.Call(Hash.START_ENTITY_FIRE, tuple.Item1);
                    this.StopFire.Set(FireTime);
                    StopFireTimer.Add(new Tuple<Ped, GTA.Timer>(tuple.Item1, this.StopFire));
                    tuple.Item2.Explode();
                    EnemyJet.RemoveAll(t => t.Item1.IsDead);
                    EnemyJet.RemoveAll(t => t.Item2.IsDead);
                    tuple.Item3.Remove();
                    
                    return;
                }
            }
        }
        void FireStop()
        {
            foreach (Tuple<Ped, GTA.Timer> tuple in StopFireTimer)
            {
                if (this.StopFire.IsOverTime)
                {
                    
                    Function.Call(Hash.STOP_ENTITY_FIRE, tuple.Item1);
                    Function.Call(Hash.ADD_EXPLOSION, tuple.Item1.Position.X, tuple.Item1.Position.Y, tuple.Item1.Position.Z, 4, 1, false, false, false);
                    StopFireTimer.RemoveAll(t => t.Item1.IsOnFire == false);
                    return;
                    //FireStop2();
                    //Model stunGunModel = new Model(WeaponHash.Molotov);
                    //Function.Call(Hash.SHOOT_SINGLE_BULLET_BETWEEN_COORDS, tuple.Item1.Position.X + 1, tuple.Item1.Position.Y, tuple.Item1.Position.Z, tuple.Item1.Position.X, tuple.Item1.Position.Y, tuple.Item1.Position.Z, 1000, true, stunGunModel.Hash, tuple.Item1, true, false, 2000f);
                    //Function.Call(Hash.ADD_EXPLOSION, tuple.Item1.Position.X, tuple.Item1.Position.Y, tuple.Item1.Position.Z, 4, 1, false, false, false);
                }
            }
        }
    }
}