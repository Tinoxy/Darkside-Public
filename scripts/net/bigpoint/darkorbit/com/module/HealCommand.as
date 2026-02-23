package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class HealCommand implements IModule
   {
      private static var _instance:HealCommand;
      
      public static const HEALTYPE_HITPOINTS:int = 0;
      
      public static const HEALTYPE_SHIELD:int = 1;
      
      public static const HEALTYPE__MAX:int = 2;
      
      public var healType:int = 0;
      
      public var healerId:int = 0;
      
      public var healedId:int = 0;
      
      public var currentHitpoints:int = 0;
      
      public var healAmount:int = 0;
      
      public function HealCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0)
      {
         super();
         this.healType = param1;
         this.healerId = param2;
         this.healedId = param3;
         this.currentHitpoints = param4;
         this.healAmount = param5;
      }
      
      public static function get instance() : HealCommand
      {
         return _instance || (_instance = new HealCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 31;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.healType = param1.readShort();
         this.healerId = param1.readInt();
         this.healerId = this.healerId >>> 13 | this.healerId << 19;
         this.healedId = param1.readInt();
         this.healedId = this.healedId << 3 | this.healedId >>> 29;
         this.currentHitpoints = param1.readInt();
         this.currentHitpoints = this.currentHitpoints << 4 | this.currentHitpoints >>> 28;
         this.healAmount = param1.readInt();
         this.healAmount = this.healAmount << 7 | this.healAmount >>> 25;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(31);
         param1.writeShort(this.healType);
         param1.writeInt(this.healerId << 13 | this.healerId >>> 19);
         param1.writeInt(this.healedId >>> 3 | this.healedId << 29);
         param1.writeInt(this.currentHitpoints >>> 4 | this.currentHitpoints << 28);
         param1.writeInt(this.healAmount >>> 7 | this.healAmount << 25);
      }
   }
}

