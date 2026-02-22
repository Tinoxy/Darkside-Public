package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class HealthModule extends PlayerAttributeModule
   {
      private static var _instance:HealthModule;
      
      public var hpMax:Number = 0;
      
      public var shield:int = 0;
      
      public var hp:Number = 0;
      
      public var nanoHull:int = 0;
      
      public var nanoHullMax:int = 0;
      
      public var shieldMax:int = 0;
      
      public function HealthModule(param1:Number = 0, param2:Number = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0)
      {
         super();
         this.hpMax = param2;
         this.shield = param3;
         this.hp = param1;
         this.nanoHull = param5;
         this.nanoHullMax = param6;
         this.shieldMax = param4;
      }
      
      public static function get instance() : HealthModule
      {
         return _instance || (_instance = new HealthModule());
      }
      
      override public function getLibcomModuleId() : int
      {
         return 19647;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 32;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
         this.hpMax = param1.readDouble();
         this.shield = param1.readInt();
         this.shield = this.shield << 16 | this.shield >>> 16;
         this.hp = param1.readDouble();
         this.nanoHull = param1.readInt();
         this.nanoHull = this.nanoHull >>> 6 | this.nanoHull << 26;
         this.nanoHullMax = param1.readInt();
         this.nanoHullMax = this.nanoHullMax >>> 8 | this.nanoHullMax << 24;
         this.shieldMax = param1.readInt();
         this.shieldMax = this.shieldMax << 14 | this.shieldMax >>> 18;
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(19647);
         super.write(param1);
         param1.writeDouble(this.hpMax);
         param1.writeInt(this.shield >>> 16 | this.shield << 16);
         param1.writeDouble(this.hp);
         param1.writeInt(this.nanoHull << 6 | this.nanoHull >>> 26);
         param1.writeInt(this.nanoHullMax << 8 | this.nanoHullMax >>> 24);
         param1.writeInt(this.shieldMax >>> 14 | this.shieldMax << 18);
      }
   }
}

