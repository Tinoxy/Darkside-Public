package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class CaptchaTriggerCommand implements IModule
   {
      private static var _instance:CaptchaTriggerCommand;
      
      public static const ECAPTCHATYPE_SOME_BLACK:int = 0;
      
      public static const ECAPTCHATYPE_ALL_BLACK:int = 1;
      
      public static const ECAPTCHATYPE_SOME_RED:int = 2;
      
      public static const ECAPTCHATYPE_ALL_RED:int = 3;
      
      public static const ECAPTCHATYPE__MAX:int = 4;
      
      public var captchaTimer:int = 0;
      
      public var redBox:int = 0;
      
      public var blackBox:int = 0;
      
      public var type:int = 0;
      
      public var captchaParam:int = 0;
      
      public var Radius:int = 0;
      
      public var posY:int = 0;
      
      public var posX:int = 0;
      
      public function CaptchaTriggerCommand(param1:int = 0, param2:int = 0, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0, param7:int = 0, param8:int = 0)
      {
         super();
         this.captchaTimer = param6;
         this.redBox = param8;
         this.blackBox = param7;
         this.type = param4;
         this.captchaParam = param5;
         this.Radius = param3;
         this.posY = param2;
         this.posX = param1;
      }
      
      public static function get instance() : CaptchaTriggerCommand
      {
         return _instance || (_instance = new CaptchaTriggerCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -7437;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 28;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.captchaTimer = param1.readInt();
         this.captchaTimer = this.captchaTimer << 10 | this.captchaTimer >>> 22;
         this.redBox = param1.readInt();
         this.redBox = this.redBox << 11 | this.redBox >>> 21;
         this.blackBox = param1.readInt();
         this.blackBox = this.blackBox >>> 9 | this.blackBox << 23;
         this.type = param1.readShort();
         this.captchaParam = param1.readInt();
         this.captchaParam = this.captchaParam << 1 | this.captchaParam >>> 31;
         this.Radius = param1.readInt();
         this.Radius = this.Radius >>> 10 | this.Radius << 22;
         this.posY = param1.readInt();
         this.posY = this.posY >>> 12 | this.posY << 20;
         this.posX = param1.readInt();
         this.posX = this.posX >>> 8 | this.posX << 24;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-7437);
         param1.writeInt(this.captchaTimer >>> 10 | this.captchaTimer << 22);
         param1.writeInt(this.redBox >>> 11 | this.redBox << 21);
         param1.writeInt(this.blackBox << 9 | this.blackBox >>> 23);
         param1.writeShort(this.type);
         param1.writeInt(this.captchaParam >>> 1 | this.captchaParam << 31);
         param1.writeInt(this.Radius << 10 | this.Radius >>> 22);
         param1.writeInt(this.posY << 12 | this.posY >>> 20);
         param1.writeInt(this.posX << 8 | this.posX >>> 24);
      }
   }
}

