using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFIndexer.BackgroundTask
{
    internal abstract class AbstractTask
    {
        /// <summary>
        /// Human-readable한 작업 이름.
        /// 예: OCR, 인덱스, 인덱스 삭제 등
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// Description은 nullable임. 반드시 사용 전 확인할 것
        /// </summary>
        public abstract string Description { get; }

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
