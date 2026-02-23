package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SeasonPassLevelPurchaseRequest implements IModule
   {
      private static var _instance:SeasonPassLevelPurchaseRequest;
      
      public var amount:int = 0;
      
      public function SeasonPassLevelPurchaseRequest(param1:int = 0)
      {
         super();
         this.amount = param1;
      }
      
      public static function get instance() : SeasonPassLevelPurchaseRequest
      {
         return _instance || (_instance = new SeasonPassLevelPurchaseRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 15201;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.amount = param1.readInt();
         this.amount = this.amount << 15 | this.amount >>> 17;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(15201);
         param1.writeInt(this.amount >>> 15 | this.amount << 17);
      }
   }
}

