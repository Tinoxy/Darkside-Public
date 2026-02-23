package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MileageMarketPackageModule implements IModule
   {
      private static var _instance:MileageMarketPackageModule;
      
      public var packagePrice:String = "";
      
      public var maxDiscountSteps:int = 0;
      
      public var packageId:String = "";
      
      public var priority:int = 0;
      
      public var discountPerStep:Number = 0;
      
      public var isBought:Boolean = false;
      
      public var claimedDiscountSteps:int = 0;
      
      public var imageKey:String = "";
      
      public function MileageMarketPackageModule(param1:String = "", param2:String = "", param3:String = "", param4:Number = 0, param5:int = 0, param6:int = 0, param7:Boolean = false, param8:int = 0)
      {
         super();
         this.packagePrice = param3;
         this.maxDiscountSteps = param5;
         this.packageId = param2;
         this.priority = param8;
         this.discountPerStep = param4;
         this.isBought = param7;
         this.claimedDiscountSteps = param6;
         this.imageKey = param1;
      }
      
      public static function get instance() : MileageMarketPackageModule
      {
         return _instance || (_instance = new MileageMarketPackageModule());
      }
      
      public function getLibcomModuleId() : int
      {
         return -28577;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 23;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.packagePrice = param1.readUTF();
         this.maxDiscountSteps = param1.readInt();
         this.maxDiscountSteps = this.maxDiscountSteps << 11 | this.maxDiscountSteps >>> 21;
         this.packageId = param1.readUTF();
         this.priority = param1.readInt();
         this.priority = this.priority << 5 | this.priority >>> 27;
         this.discountPerStep = param1.readFloat();
         this.isBought = param1.readBoolean();
         this.claimedDiscountSteps = param1.readInt();
         this.claimedDiscountSteps = this.claimedDiscountSteps << 13 | this.claimedDiscountSteps >>> 19;
         this.imageKey = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-28577);
         param1.writeUTF(this.packagePrice);
         param1.writeInt(this.maxDiscountSteps >>> 11 | this.maxDiscountSteps << 21);
         param1.writeUTF(this.packageId);
         param1.writeInt(this.priority >>> 5 | this.priority << 27);
         param1.writeFloat(this.discountPerStep);
         param1.writeBoolean(this.isBought);
         param1.writeInt(this.claimedDiscountSteps >>> 13 | this.claimedDiscountSteps << 19);
         param1.writeUTF(this.imageKey);
      }
   }
}

