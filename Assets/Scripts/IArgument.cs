﻿namespace MoveToCode {
    public abstract class IArgument {
        CodeBlock myCodeBlock;

        public abstract IDataType EvaluateArgument();
        public virtual void ResestInternalState() { }
        public abstract int GetNumArguments();
        public abstract string DescriptiveInstructionToString();

        public CodeBlock GetCodeBlock() {
            return myCodeBlock;
        }

        public IArgument(CodeBlock cbIn) {
            myCodeBlock = cbIn;
            ResestInternalState();
        }
    }
}