import './Section.css'
const Section = ({ title, children }) => (
  <section className="dashboard-section">
    <h2 className="section-title">{title}</h2>
    <div className="section-grid">
      {children}
    </div>
  </section>
); 
export default Section;