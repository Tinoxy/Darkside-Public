package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class RogueLiteModifierModule implements IModule
   {
      private static var _instance:RogueLiteModifierModule;
      
      public var modifierId:int = 0;
      
      public function RogueLiteModifierModule(param1:int = 0)
      {
         super();
         this.modifierId = param1;
      }
      
      public static function get instance() : RogueLiteModifierModule
      {
         return _instance || (_instance = new RogueLiteModifierModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -2923;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.modifierId = param1.readInt();
         this.modifierId = this.modifierId >>> 13 | this.modifierId << 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-2923);
         param1.writeInt(this.modifierId << 13 | this.modifierId >>> 19);
      }
   }
}

