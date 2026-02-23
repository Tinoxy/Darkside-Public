package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class AgatusEventInfoCommand implements IModule
   {
      private static var _instance:AgatusEventInfoCommand;
      
      public var npcID:int = 0;
      
      public var textKey:String = "";
      
      public var type:int = 0;
      
      public var bossInfoCommand:AgatusEventBossInfoCommand;
      
      public var timeInfoCommand:AgatusEventTimeInfoCommand;
      
      public function AgatusEventInfoCommand(param1:int = 0, param2:String = "", param3:int = 0, param4:AgatusEventTimeInfoCommand = null, param5:AgatusEventBossInfoCommand = null)
      {
         super();
         this.npcID = param3;
         this.textKey = param2;
         this.type = param1;
         if(param5 == null)
         {
            this.bossInfoCommand = new AgatusEventBossInfoCommand();
         }
         else
         {
            this.bossInfoCommand = param5;
         }
         if(param4 == null)
         {
            this.timeInfoCommand = new AgatusEventTimeInfoCommand();
         }
         else
         {
            this.timeInfoCommand = param4;
         }
      }
      
      public static function get instance() : AgatusEventInfoCommand
      {
         return _instance || (_instance = new AgatusEventInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -15892;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.npcID = param1.readInt();
         this.npcID = this.npcID >>> 9 | this.npcID << 23;
         this.textKey = param1.readUTF();
         this.type = param1.readShort();
         this.bossInfoCommand = AgatusEventBossInfoCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.bossInfoCommand)
         {
            this.bossInfoCommand.read(param1);
         }
         this.timeInfoCommand = AgatusEventTimeInfoCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.timeInfoCommand)
         {
            this.timeInfoCommand.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-15892);
         param1.writeInt(this.npcID << 9 | this.npcID >>> 23);
         param1.writeUTF(this.textKey);
         param1.writeShort(this.type);
         if(null != this.bossInfoCommand)
         {
            this.bossInfoCommand.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.timeInfoCommand)
         {
            this.timeInfoCommand.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

