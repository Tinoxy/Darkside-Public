package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GroupMemberInitializationCommand implements IModule
   {
      private static var _instance:GroupMemberInitializationCommand;
      
      public var cloaked:Boolean = false;
      
      public var health:HealthModule;
      
      public var loggedOut:Boolean = false;
      
      public var shipIcon:ShipIconModule;
      
      public var raidGateGameStage:RaidGateGameStageModule;
      
      public var id:int = 0;
      
      public var level:int = 0;
      
      public var target:TargetModule;
      
      public var blockingInvitations:Boolean = false;
      
      public var name:String = "";
      
      public var active:Boolean = false;
      
      public var clan:ClanModule;
      
      public var faction:FactionModule;
      
      public var inFight:Boolean = false;
      
      public var location:LocationModule;
      
      public function GroupMemberInitializationCommand(param1:String = "", param2:int = 0, param3:HealthModule = null, param4:LocationModule = null, param5:int = 0, param6:Boolean = false, param7:Boolean = false, param8:Boolean = false, param9:Boolean = false, param10:Boolean = false, param11:ClanModule = null, param12:FactionModule = null, param13:TargetModule = null, param14:ShipIconModule = null, param15:RaidGateGameStageModule = null)
      {
         super();
         this.cloaked = param7;
         if(param3 == null)
         {
            this.health = new HealthModule();
         }
         else
         {
            this.health = param3;
         }
         this.loggedOut = param9;
         if(param14 == null)
         {
            this.shipIcon = new ShipIconModule();
         }
         else
         {
            this.shipIcon = param14;
         }
         if(param15 == null)
         {
            this.raidGateGameStage = new RaidGateGameStageModule();
         }
         else
         {
            this.raidGateGameStage = param15;
         }
         this.id = param2;
         this.level = param5;
         if(param13 == null)
         {
            this.target = new TargetModule();
         }
         else
         {
            this.target = param13;
         }
         this.blockingInvitations = param10;
         this.name = param1;
         this.active = param6;
         if(param11 == null)
         {
            this.clan = new ClanModule();
         }
         else
         {
            this.clan = param11;
         }
         if(param12 == null)
         {
            this.faction = new FactionModule();
         }
         else
         {
            this.faction = param12;
         }
         this.inFight = param8;
         if(param4 == null)
         {
            this.location = new LocationModule();
         }
         else
         {
            this.location = param4;
         }
      }
      
      public static function get instance() : GroupMemberInitializationCommand
      {
         return _instance || (_instance = new GroupMemberInitializationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -27889;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 15;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.cloaked = param1.readBoolean();
         this.health = HealthModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.health)
         {
            this.health.read(param1);
         }
         this.loggedOut = param1.readBoolean();
         this.shipIcon = ShipIconModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.shipIcon)
         {
            this.shipIcon.read(param1);
         }
         this.raidGateGameStage = RaidGateGameStageModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.raidGateGameStage)
         {
            this.raidGateGameStage.read(param1);
         }
         this.id = param1.readInt();
         this.id = this.id << 10 | this.id >>> 22;
         this.level = param1.readInt();
         this.level = this.level << 11 | this.level >>> 21;
         this.target = TargetModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.target)
         {
            this.target.read(param1);
         }
         this.blockingInvitations = param1.readBoolean();
         this.name = param1.readUTF();
         this.active = param1.readBoolean();
         this.clan = ClanModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.clan)
         {
            this.clan.read(param1);
         }
         this.faction = FactionModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.faction)
         {
            this.faction.read(param1);
         }
         this.inFight = param1.readBoolean();
         this.location = LocationModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.location)
         {
            this.location.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-27889);
         param1.writeBoolean(this.cloaked);
         if(null != this.health)
         {
            this.health.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeBoolean(this.loggedOut);
         if(null != this.shipIcon)
         {
            this.shipIcon.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.raidGateGameStage)
         {
            this.raidGateGameStage.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeInt(this.id >>> 10 | this.id << 22);
         param1.writeInt(this.level >>> 11 | this.level << 21);
         if(null != this.target)
         {
            this.target.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeBoolean(this.blockingInvitations);
         param1.writeUTF(this.name);
         param1.writeBoolean(this.active);
         if(null != this.clan)
         {
            this.clan.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.faction)
         {
            this.faction.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeBoolean(this.inFight);
         if(null != this.location)
         {
            this.location.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

