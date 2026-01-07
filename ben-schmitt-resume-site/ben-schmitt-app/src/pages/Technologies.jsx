import Aside from "../components/aside";


export default function Technologies(){
    return(
        <main className="main-with-aside">
          <div class="flow">
      
            <h1>Technologies Used</h1>
            <div className="filter">
              <h2 className="visually-hidden">Filter by type of conference</h2>
              <button className='filter-btn active' data-filter="all">All</button>
              <button className='filter-btn' data-filter="frontend">Frontend</button>
              <button className='filter-btn' data-filter="backend">Backend</button>
              <button className='filter-btn' data-filter="fullstack">Fullstack</button>
              <button className='filter-btn' data-filter="dataengineering">Data Engineering</button>
            </div>
      
            <ul className="technology-list">
              <li className="technology" data-category='fullstack'>
                <div className="technology-details">
                  <span className="technology-date">January 30th - February 1st</span>
                  <span className="technology-category">Fullstack</span>
                </div>
                <h2 className="technology-name">THAT Conference Texas</h2>
              </li>
      
              <li className="technology" data-category='fullstack'>
                <div className="technology-details">
                  <span className="technology-date">February 29th - March 1st</span>
                  <span className="technology-category">Fullstack</span>
                </div>
                <h2 className="technology-name">DEVWorld</h2>
              </li>
      
              <li className="technology" data-category='backend'>
                <div className="technology-details">
                  <span className="technology-date">March 19th - March 23rd</span>
                  <span className="technology-category">Backend</span>
                </div>
                <h2 className="technology-name">SQLBits</h2>
              </li>
      
              <li className="technology" data-category='frontend'>
                <div className="technology-details">
                  <span className="technology-date">June 6th - June 7th</span>
                  <span className="technology-category">Frontend</span>
                </div>
                <h2 className="technology-name">CSS Day</h2>
              </li>
      
              <li className="technology" data-category='dataengineering'>
                <div className="technology-details">
                  <span className="technology-date">September 9th - September 11th</span>
                  <span className="technology-category">Frontend</span>
                </div>
                <h2 className="technology-name">SmashingConf Freiburg</h2>
              </li>
              <li className="technology" data-category='dataengineering'>
                <div className="technology-details">
                  <span className="technology-date">September 9th - September 11th</span>
                  <span className="technology-category">Frontend</span>
                </div>
                <h2 className="technology-name">SmashingConf Freiburg</h2>
              </li>
            </ul>
          </div>
          <Aside/>
        </main>
    );
}