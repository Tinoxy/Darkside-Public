package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MileageMarketPackageUpdateCommand implements IModule
   {
      private static var _instance:MileageMarketPackageUpdateCommand;
      
      public var claimedDiscountSteps:int = 0;
      
      public var isBought:Boolean = false;
      
      public var packageID:String = "";
      
      public function MileageMarketPackageUpdateCommand(param1:String = "", param2:int = 0, param3:Boolean = false)
      {
         super();
         this.claimedDiscountSteps = param2;
         this.isBought = param3;
         this.packageID = param1;
      }
      
      public static function get instance() : MileageMarketPackageUpdateCommand
      {
         return _instance || (_instance = new MileageMarketPackageUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -5613;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 7;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.claimedDiscountSteps = param1.readInt();
         this.claimedDiscountSteps = this.claimedDiscountSteps >>> 13 | this.claimedDiscountSteps << 19;
         this.isBought = param1.readBoolean();
         this.packageID = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-5613);
         param1.writeInt(this.claimedDiscountSteps << 13 | this.claimedDiscountSteps >>> 19);
         param1.writeBoolean(this.isBought);
         param1.writeUTF(this.packageID);
      }
   }
}

