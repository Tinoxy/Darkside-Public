package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class HintWindowAddComand implements IModule
   {
      private static var _instance:HintWindowAddComand;
      
      public static const HINTWINDOWVISIBILITY_OPEN:int = 0;
      
      public static const HINTWINDOWVISIBILITY_NOTIFICATION_BAR:int = 1;
      
      public static const HINTWINDOWVISIBILITY_HINTLIST:int = 2;
      
      public static const HINTWINDOWVISIBILITY__MAX:int = 3;
      
      public var visibility:int = 0;
      
      public var openedFromArchive:Boolean = false;
      
      public var addToList:Boolean = false;
      
      public var paymentLink:String = "";
      
      public var window:HintWindowModule;
      
      public var soundModule:SoundTriggerModule;
      
      public function HintWindowAddComand(param1:HintWindowModule = null, param2:Boolean = false, param3:int = 0, param4:Boolean = false, param5:SoundTriggerModule = null, param6:String = "")
      {
         super();
         this.visibility = param3;
         this.openedFromArchive = param4;
         this.addToList = param2;
         this.paymentLink = param6;
         if(param1 == null)
         {
            this.window = new HintWindowModule();
         }
         else
         {
            this.window = param1;
         }
         if(param5 == null)
         {
            this.soundModule = new SoundTriggerModule();
         }
         else
         {
            this.soundModule = param5;
         }
      }
      
      public static function get instance() : HintWindowAddComand
      {
         return _instance || (_instance = new HintWindowAddComand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -27417;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 4;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.visibility = param1.readShort();
         this.openedFromArchive = param1.readBoolean();
         this.addToList = param1.readBoolean();
         this.paymentLink = param1.readUTF();
         this.window = HintWindowModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.window)
         {
            this.window.read(param1);
         }
         this.soundModule = SoundTriggerModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.soundModule)
         {
            this.soundModule.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-27417);
         param1.writeShort(this.visibility);
         param1.writeBoolean(this.openedFromArchive);
         param1.writeBoolean(this.addToList);
         param1.writeUTF(this.paymentLink);
         if(null != this.window)
         {
            this.window.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.soundModule)
         {
            this.soundModule.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

