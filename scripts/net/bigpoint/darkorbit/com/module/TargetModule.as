package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class TargetModule extends PlayerAttributeModule
   {
      private static var _instance:TargetModule;
      
      public var shipType:ShipIconModule;
      
      public var health:HealthModule;
      
      public var name:String = "";
      
      public function TargetModule(param1:ShipIconModule = null, param2:String = "", param3:HealthModule = null)
      {
         super();
         if(param1 == null)
         {
            this.shipType = new ShipIconModule();
         }
         else
         {
            this.shipType = param1;
         }
         if(param3 == null)
         {
            this.health = new HealthModule();
         }
         else
         {
            this.health = param3;
         }
         this.name = param2;
      }
      
      public static function get instance() : TargetModule
      {
         return _instance || (_instance = new TargetModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return -28833;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.shipType = ShipIconModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.shipType)
         {
            this.shipType.read(param1);
         }
         this.health = HealthModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.health)
         {
            this.health.read(param1);
         }
         this.name = param1.readUTF();
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-28833);
         super.write(param1);
         if(null != this.shipType)
         {
            this.shipType.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.health)
         {
            this.health.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeUTF(this.name);
      }
   }
}

