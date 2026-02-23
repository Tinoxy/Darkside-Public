package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TdmResultCommand implements IModule
   {
      private static var _instance:TdmResultCommand;
      
      public var targetScore:int = 0;
      
      public var winner:FactionModule;
      
      public var loserScore:int = 0;
      
      public var matchDurationInSeconds:int = 0;
      
      public var loser:FactionModule;
      
      public var winnerScore:int = 0;
      
      public function TdmResultCommand(param1:FactionModule = null, param2:FactionModule = null, param3:int = 0, param4:int = 0, param5:int = 0, param6:int = 0)
      {
         super();
         this.targetScore = param5;
         if(param1 == null)
         {
            this.winner = new FactionModule();
         }
         else
         {
            this.winner = param1;
         }
         this.loserScore = param4;
         this.matchDurationInSeconds = param6;
         if(param2 == null)
         {
            this.loser = new FactionModule();
         }
         else
         {
            this.loser = param2;
         }
         this.winnerScore = param3;
      }
      
      public static function get instance() : TdmResultCommand
      {
         return _instance || (_instance = new TdmResultCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 24301;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.targetScore = param1.readInt();
         this.targetScore = this.targetScore << 7 | this.targetScore >>> 25;
         this.winner = FactionModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.winner)
         {
            this.winner.read(param1);
         }
         this.loserScore = param1.readInt();
         this.loserScore = this.loserScore << 16 | this.loserScore >>> 16;
         this.matchDurationInSeconds = param1.readInt();
         this.matchDurationInSeconds = this.matchDurationInSeconds >>> 1 | this.matchDurationInSeconds << 31;
         this.loser = FactionModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.loser)
         {
            this.loser.read(param1);
         }
         this.winnerScore = param1.readInt();
         this.winnerScore = this.winnerScore >>> 11 | this.winnerScore << 21;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(24301);
         param1.writeInt(this.targetScore >>> 7 | this.targetScore << 25);
         if(null != this.winner)
         {
            this.winner.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeInt(this.loserScore >>> 16 | this.loserScore << 16);
         param1.writeInt(this.matchDurationInSeconds << 1 | this.matchDurationInSeconds >>> 31);
         if(null != this.loser)
         {
            this.loser.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeInt(this.winnerScore << 11 | this.winnerScore >>> 21);
      }
   }
}

