package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class VIPMerchantExchangePackageUpdateCommand implements IModule
   {
      private static var _instance:VIPMerchantExchangePackageUpdateCommand;
      
      public var packagesBought:int = 0;
      
      public function VIPMerchantExchangePackageUpdateCommand(param1:int = 0)
      {
         super();
         this.packagesBought = param1;
      }
      
      public static function get instance() : VIPMerchantExchangePackageUpdateCommand
      {
         return _instance || (_instance = new VIPMerchantExchangePackageUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -8262;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.packagesBought = param1.readShort();
         this.packagesBought = 65535 & ((65535 & this.packagesBought) >>> 13 | (65535 & this.packagesBought) << 3);
         this.packagesBought = this.packagesBought > 32767 ? int(this.packagesBought - 65536) : this.packagesBought;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-8262);
         param1.writeShort(65535 & ((65535 & this.packagesBought) << 13 | (65535 & this.packagesBought) >>> 3));
      }
   }
}

