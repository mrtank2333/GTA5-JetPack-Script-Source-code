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
        Ped player;//设player是主角
        Vector3 PlayerPos;//Player位置
        Ped FriendlyJetPackDriver = null;//声明FriendlyJetPackDriver全局变量
        Vehicle FriendlyJetPack = null;//声明FriendlyJetPack全局变量
        Ped EnemyJetPackDriver = null;//声明EnemyJetPackDriver全局变量
        Vehicle EnemyJetPack = null;//声明EnemyJetPack全局变量
        Blip Friendlyblip;
        Blip Enemyblip;
        float nX;
        float nY;

        List<Tuple<Ped, Vehicle, Blip>> FriendlyJet = new List<Tuple<Ped, Vehicle, Blip>>();//(创建列表以记录刷出的友军人物和载具)
        List<Tuple<Ped, Vehicle, Blip>> EnemyJet = new List<Tuple<Ped, Vehicle, Blip>>();//(创建列表以记录刷出的友军人物和载具)
        //读取ini文件及设置string全局变量
        ScriptSettings config;
        Keys OpenMenu;
        string FriendlyPedModelName;
        string EnemyPedModelName;
        //自动选择语言模块全局变量
        int L;
        string[] title1 = {"JetPack", "JetPack", "JetPack", "JetPack", "JetPack", "JetPack", "JetPack", "реактивный пакет", "JetPack", "火箭飛行兵", "ジェットパック", "JetPack", "火箭飞行兵" };
        string[] title2 = { "Menu", "Menu", "Menü", "Menu", "Menú", "Menu", "Menu", "меню", "Menu", "菜單", "メニュー", "Menú", "菜单" };
        string[] menu1 = { "GiveJetPack", "DonnerJetPack", "GebenJetPack", "DaiJetPack", "DarJetPack", "Dê JetPack", "Dodaj JetPack", "давать реактивный пакет", "JetPack주다", "給噴射背包", "ジェットパックを作成", "DarJetPack", "刷出喷气背包" };
        string[] menu2 = { "SpawnFriendlyJetPack", "SpawnAmicalJetPack", "GenerierenFreundlichJetPack", "GeneraFriendlyJetPack", "CrearAmigableJetPack", "CriarAmigávelJetPack", "UtwórzPrzyjaznyJetPack", "Создать дружелюбный", "우호적JetPack생성", "創建友軍飛行兵", "友軍を作成", "CrearAmigableJetPack", "刷出友军火箭飞行兵" };
        string[] menu3 = { "SpawnEnemyJetPack", "SpawnEnnemiJetPack", "GenerierenFeindJetPack", "GeneraNemicoJetPack", "CrearEnemigoJetPack", "CriarInimigoJetPack", "UtwórzWrógJetPack", "Создать враг", "적JetPack생성", "創建敵軍飛行兵", "敵を作成", "CrearEnemigoJetPack", "刷出敌军火箭飞行兵" };
        string[] menu4 = { "KillFriendlyJetPack", "TuerAmicalJetPack", "TötenFreundlichJetPack", "UccidiFriendlyJetPack", "MatarAmigableJetPack", "MatarAmigávelJetPack", "ZabijPrzyjaznyJetPack", "убить дружелюбный", "우호적JetPack죽이다", "殺死友軍飛行兵", "友軍を殺す", "MatarAmigableJetPack", "杀死友军火箭飞行兵" };
        string[] menu5 = { "KillEnemyJetPack", "TuerEnnemiJetPack", "TötenFeindJetPack", "UccidiNemicoJetPack", "MatarEnemigoJetPack", "MatarInimigoJetPack", "ZabijWrógJetPack", "убить враг", "적JetPack죽이다", "殺死敵軍飛行兵", "敵を殺す", "MatarEnemigoJetPack", "杀死敌军火箭飞行兵" };
        string[] Spawed1 = { "JetPack Spawed!!!", "JetPack Spawed!!!", "JetPackGeneriert!!!", "GeneratoJetPack!!!", "CrearJetPackCompletar!!!", "JetPackCriado!!!", "StworzoneJetPack!!!", "Создание завершено！！！", "JetPack생성완성", "已創建噴射背包！！！", "ジェットパックを作成した！！！", "CrearJetPackCompletar!", "喷气背包已刷出！！！" };
        string[] Spawed2 = { "FriendlyJetPack Spawed!!!", "AmicalJetPack Spawed!!!", "FreundlichJetPackGeneriert!!!", "GeneratoFriendlyJetPack!!!", "CrearAmigableCompletar!!!", "AmigávelJetPackCriado!!!", "StworzonePrzyjaznyJetPack!!!", "дружелюбный Создание завершено！！！", "우호적JetPack생성완성", "已創建友軍飛行兵！！！", "友軍を作成した！！！", "CrearAmigableCompletar!", "已刷出友军火箭飞行兵！！！" };
        string[] Spawed3 = { "EnemyJetPack Spawed!!!", "EnnemiJetPack Spawed!!!", "JetPackGeneriert!!!", "GeneratoNemicoJetPack!!!", "CrearrEnemigoCompletar!!!", "InimigoJetPackCriado!!!", "StworzoneWrógJetPack!!!", "враг Создание завершено！！！", "적JetPack생성완성", "已創建敵軍飛行兵!!!", "敵を作成した！！！", "CrearrEnemigoCompletar!", "已刷出敌军火箭飞行兵！！！" };
        //GTA NativeUI菜单
        MenuPool modMenuPool;
        UIMenu mainMenu;
        UIMenuItem giveJetPack;
        UIMenuItem spawnFriendlyJetPack;
        UIMenuItem spawnEnemyJetPack;
        UIMenuItem killFriendlyJetPack;
        UIMenuItem killEnemyJetPack;
        public JetPack()
        {
            //自动选择语言
            int L = Function.Call<int>(Hash._GET_UI_LANGUAGE_ID);
            //读取配置文件部分
            config = ScriptSettings.Load("scripts\\JetPackSettings.ini");//读取配置文件
            OpenMenu = config.GetValue<Keys>("Options", "OpenMenu", Keys.F5); //读取热键内容
            FriendlyPedModelName = config.GetValue<string>("Peds1", "FriendlyPedModelName", "s_m_m_highsec_01");//读取友军人物模型名称
            EnemyPedModelName = config.GetValue<string>("Peds2", "EnemyPedModelName", "s_m_m_chemsec_01‬");//读取敌军人物模型名称
            //创建NativeUI菜单及菜单选项
            modMenuPool = new MenuPool();
            mainMenu = new UIMenu(title1[L], title2[L]);
            modMenuPool.Add(mainMenu);

            giveJetPack = new UIMenuItem(menu1[L]);
            mainMenu.AddItem(giveJetPack);

            spawnFriendlyJetPack = new UIMenuItem(menu2[L]);
            mainMenu.AddItem(spawnFriendlyJetPack);

            spawnEnemyJetPack = new UIMenuItem(menu3[L]);
            mainMenu.AddItem(spawnEnemyJetPack);

            killFriendlyJetPack = new UIMenuItem(menu4[L]);
            mainMenu.AddItem(killFriendlyJetPack);

            killEnemyJetPack = new UIMenuItem(menu5[L]);
            mainMenu.AddItem(killEnemyJetPack);

            mainMenu.OnItemSelect += onMainMenuItemSelect;

            KeyDown += OnKeyDown;//按键按下事件
            Tick += OnTick;
            Tick += OnTick2;
            Tick += OnTick3;
        }
        void onMainMenuItemSelect(UIMenu sender, UIMenuItem item, int index)
        {
            if(item == giveJetPack)
            {
                GiveJetPack();
            }
            else if(item == spawnFriendlyJetPack)
            {
                SpawnFriendlyJetPack();
            }else if(item == spawnEnemyJetPack)
            {
                SpawnEnemyJetPack();
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
            }else if(e.KeyCode == OpenMenu && modMenuPool.IsAnyMenuOpen())
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
                if (FriendlyJetPack == null&& EnemyJetPack == null)
                {

                }
                else if (EnemyJetPack == null)
                {
                    AttackEnemy();
                }
                else if (FriendlyJetPack == null) 
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
                if (tuple.Item1.IsDead || tuple.Item2.IsDead)
                {
                    tuple.Item3.Remove();
                }
                Vector3 FriendlyJetPackPos = tuple.Item1.Position;//获取友军坐标
                float DistanceA = FriendlyJetPackPos.X - PlayerPos.X;
                float DistanceB = FriendlyJetPackPos.Y - PlayerPos.Y;
                if (DistanceA > 500f || DistanceB > 500f)
                {
                    FriendlyJetPackGotoPlayer();
                }
            }
            foreach (Tuple<Ped, Vehicle, Blip> tuple in EnemyJet)
            {
                if (tuple.Item1.IsDead || tuple.Item2.IsDead)
                {
                    tuple.Item3.Remove();
                }
            }
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
        void SpawnFriendlyJetPack()
        {
            float n1 = -25f + nX;
            float n2 = -26f + nX;
            Vector3 FriendlyJetPackPos = Game.Player.Character.Position + Game.Player.Character.UpVector * 40f + Game.Player.Character.ForwardVector * n1;//喷气背包刷出位置
            FriendlyJetPack = World.CreateVehicle(new Model(VehicleHash.Thruster), FriendlyJetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, FriendlyJetPack, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, FriendlyJetPack, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, FriendlyJetPack, 10, 1, true);//给予喷气背包导弹的改装件
        
            Vector3 FriendlyJetPackDriverPos = Game.Player.Character.Position + Game.Player.Character.UpVector * 40f + Game.Player.Character.ForwardVector * n2;//喷气背包驾驶员JetPackDriver刷出位置
            FriendlyJetPackDriver = World.CreatePed(FriendlyPedModelName, FriendlyJetPackDriverPos);//喷气背包驾驶员JetPackDriver刷出
            Function.Call(Hash.SET_PED_INTO_VEHICLE, FriendlyJetPackDriver, FriendlyJetPack, -1);//JetPackDriver进入喷气背包驾驶位
            FriendlyJetPackDriver.Task.FightAgainstHatedTargets(500f);//对抗半径500米的一切敌对目标
            FriendlyJetPackDriver.AlwaysKeepTask = true;//始终保持任务
            FriendlyJetPackDriver.Health = 1500;//生命值1500
            FriendlyJetPackDriver.Armor = 200;//护甲100
            FriendlyJetPackDriver.RelationshipGroup = Game.Player.Character.RelationshipGroup;//设置喷气背包驾驶员JetPackDriver属于主角阵营
            Friendlyblip = FriendlyJetPack.AddBlip();
            Friendlyblip.Sprite = (BlipSprite)597;//强制转换枚举
            FriendlyJetPack.CurrentBlip.Color = BlipColor.Blue;
            FriendlyJetPack.CurrentBlip.Scale = 0.6f;

            FriendlyJet.Add(new Tuple<Ped, Vehicle, Blip>(FriendlyJetPackDriver, FriendlyJetPack, Friendlyblip));
            
            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, FriendlyJetPackDriver, FriendlyJetPack, PlayerPos.X, PlayerPos.Y, PlayerPos.Z, 30f, 1f, 1489874736, 16777216, 1f, true);
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
                tuple.Item1.Kill();
                tuple.Item2.Explode();
                FriendlyJet.RemoveAll(t => t.Item1.IsDead);
                FriendlyJet.RemoveAll(t => t.Item2.IsDead);
                tuple.Item3.Remove();
                return;
            }
        }
        void SpawnEnemyJetPack()
        {
            float n1 = 100f + nX;
            float n2 = 40f + nY;
            float n3 = 101f + nX;
            float n4 = 40f + nY;
            Vector3 EnemyJetPackPos = Game.Player.Character.Position + Game.Player.Character.UpVector * n2 + Game.Player.Character.ForwardVector * n1;//喷气背包刷出位置
            EnemyJetPack = World.CreateVehicle(new Model(VehicleHash.Thruster), EnemyJetPackPos);//喷气背包刷出
            Function.Call(Hash.SET_VEHICLE_ENGINE_ON, EnemyJetPack, true);
            Function.Call(Hash.SET_VEHICLE_MOD_KIT, EnemyJetPack, 0);
            Function.Call(Hash.SET_VEHICLE_MOD, EnemyJetPack, 10, 1, true);//给予喷气背包导弹的改装件

            Vector3 EnemyJetPackDriverPos = Game.Player.Character.Position + Game.Player.Character.UpVector * n4 + Game.Player.Character.ForwardVector * n3;//喷气背包驾驶员JetPackDriver刷出位置
            EnemyJetPackDriver = World.CreatePed(EnemyPedModelName, EnemyJetPackDriverPos);//喷气背包驾驶员JetPackDriver刷出
            Function.Call(Hash.SET_PED_INTO_VEHICLE, EnemyJetPackDriver, EnemyJetPack, -1);//JetPackDriver进入喷气背包驾驶位

            EnemyJetPackDriver.AlwaysKeepTask = true;//始终保持任务
            EnemyJetPackDriver.Health = 1500;//生命值1500
            EnemyJetPackDriver.Armor = 200;//护甲100
            int EnemyRelationShipGroup = Function.Call<int>(Hash.GET_HASH_KEY, "HATES_PLAYER"); //创建恨主角的阵营指数
            Function.Call(Hash.SET_PED_RELATIONSHIP_GROUP_HASH, EnemyJetPackDriver, EnemyRelationShipGroup);//设置阵营为敌对阵营
            Enemyblip = EnemyJetPack.AddBlip();
            Enemyblip.Sprite = (BlipSprite)597;//强制转换枚举
            EnemyJetPack.CurrentBlip.Color = BlipColor.Red;
            EnemyJetPack.CurrentBlip.Scale = 0.6f;

            EnemyJet.Add(new Tuple<Ped, Vehicle, Blip>(EnemyJetPackDriver, EnemyJetPack, Enemyblip));
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
                    if (p.IsAlive && p.IsHuman && IsPedFriendly(p)&&p.IsPlayer == false)//数组中是否有是人类的活着的友军目标
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
                tuple.Item1.Kill();
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
    }
}
