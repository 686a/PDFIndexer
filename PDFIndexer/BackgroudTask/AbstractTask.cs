using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer.BackgroundTask
{
    internal abstract class AbstractTask
    {
        public abstract void Run();

        /// <summary>
        /// 같은 종류의 작업끼리 중복을 막기 위해 사용되는 작업 별 고유한 해시
        /// <para>
        /// 만약 IndexTask가 있을 때, 클래스 내부의 Path가 해당 작업의 해시가 됨.
        /// 같은 작업 내에서 같은 해시가 발견되면, 그 작업은 종료 전까지 실행되지 않음.
        /// 
        /// 서로 다른 작업 내에선 해시가 같아도 작업 실행에 문제 없음.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public abstract string GetTaskHash();
    }
}
