package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ClanRelationChangedCommand implements IModule
   {
      private static var _instance:ClanRelationChangedCommand;
      
      public var newRelation:ClanRelationModule;
      
      public var clanId:int = 0;
      
      public var userId:int = 0;
      
      public function ClanRelationChangedCommand(param1:ClanRelationModule = null, param2:int = 0, param3:int = 0)
      {
         super();
         if(param1 == null)
         {
            this.newRelation = new ClanRelationModule();
         }
         else
         {
            this.newRelation = param1;
         }
         this.clanId = param3;
         this.userId = param2;
      }
      
      public static function get instance() : ClanRelationChangedCommand
      {
         return _instance || (_instance = new ClanRelationChangedCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 692;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.newRelation = ClanRelationModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.newRelation)
         {
            this.newRelation.read(param1);
         }
         this.clanId = param1.readInt();
         this.clanId = this.clanId >>> 7 | this.clanId << 25;
         this.userId = param1.readInt();
         this.userId = this.userId << 11 | this.userId >>> 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(692);
         if(null != this.newRelation)
         {
            this.newRelation.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeInt(this.clanId << 7 | this.clanId >>> 25);
         param1.writeInt(this.userId >>> 11 | this.userId << 21);
      }
   }
}

