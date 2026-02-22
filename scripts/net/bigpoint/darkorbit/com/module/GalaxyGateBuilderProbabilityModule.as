package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class GalaxyGateBuilderProbabilityModule implements IModule
   {
      private static var _instance:GalaxyGateBuilderProbabilityModule;
      
      public static const CATEGORY_INVALID:int = 0;
      
      public static const CATEGORY_AMMUNITION:int = 1;
      
      public static const CATEGORY_RESOURCE:int = 2;
      
      public static const CATEGORY_VOUCHER:int = 3;
      
      public static const CATEGORY_PART:int = 4;
      
      public static const CATEGORY_SPECIAL:int = 5;
      
      public static const CATEGORY_LOGFILE:int = 6;
      
      public static const CATEGORY__MAX:int = 7;
      
      public var percentage:int = 0;
      
      public var category:int = 0;
      
      public function GalaxyGateBuilderProbabilityModule(param1:int = 0, param2:int = 0)
      {
         super();
         this.percentage = param2;
         this.category = param1;
      }
      
      public static function get instance() : GalaxyGateBuilderProbabilityModule
      {
         return _instance || (_instance = new GalaxyGateBuilderProbabilityModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -2325;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 1;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.percentage = param1.readByte();
         this.percentage = 255 & ((255 & this.percentage) << 6 | (255 & this.percentage) >>> 2);
         this.percentage = this.percentage > 127 ? int(this.percentage - 256) : this.percentage;
         this.category = param1.readShort();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-2325);
         param1.writeByte(255 & ((255 & this.percentage) >>> 6 | (255 & this.percentage) << 2));
         param1.writeShort(this.category);
      }
   }
}

