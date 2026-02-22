package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class UridiumBankInfoCommand implements IModule
   {
      private static var _instance:UridiumBankInfoCommand;
      
      public var secondsLeft:int = 0;
      
      public var price:String = "";
      
      public var currentValue:int = 0;
      
      public var maxValue:int = 0;
      
      public var imgKey:String = "";
      
      public var textKey:String = "";
      
      public function UridiumBankInfoCommand(param1:int = 0, param2:int = 0, param3:String = "", param4:int = 0, param5:String = "", param6:String = "")
      {
         super();
         this.secondsLeft = param4;
         this.price = param3;
         this.currentValue = param1;
         this.maxValue = param2;
         this.imgKey = param6;
         this.textKey = param5;
      }
      
      public static function get instance() : UridiumBankInfoCommand
      {
         return _instance || (_instance = new UridiumBankInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -17289;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 18;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.secondsLeft = param1.readInt();
         this.secondsLeft = this.secondsLeft << 13 | this.secondsLeft >>> 19;
         this.price = param1.readUTF();
         this.currentValue = param1.readInt();
         this.currentValue = this.currentValue << 2 | this.currentValue >>> 30;
         this.maxValue = param1.readInt();
         this.maxValue = this.maxValue >>> 9 | this.maxValue << 23;
         this.imgKey = param1.readUTF();
         this.textKey = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-17289);
         param1.writeInt(this.secondsLeft >>> 13 | this.secondsLeft << 19);
         param1.writeUTF(this.price);
         param1.writeInt(this.currentValue >>> 2 | this.currentValue << 30);
         param1.writeInt(this.maxValue << 9 | this.maxValue >>> 23);
         param1.writeUTF(this.imgKey);
         param1.writeUTF(this.textKey);
      }
   }
}

