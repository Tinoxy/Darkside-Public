package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class RankRewardRangeModule implements IModule
   {
      private static var _instance:RankRewardRangeModule;
      
      public var from:int = 0;
      
      public var to:int = 0;
      
      public function RankRewardRangeModule(param1:int = 0, param2:int = 0)
      {
         super();
         this.from = param1;
         this.to = param2;
      }
      
      public static function get instance() : RankRewardRangeModule
      {
         return _instance || (_instance = new RankRewardRangeModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return 27357;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.from = param1.readInt();
         this.from = this.from << 6 | this.from >>> 26;
         this.to = param1.readInt();
         this.to = this.to >>> 16 | this.to << 16;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(27357);
         param1.writeInt(this.from >>> 6 | this.from << 26);
         param1.writeInt(this.to << 16 | this.to >>> 16);
      }
   }
}

