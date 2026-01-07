import Aside from "../components/aside";

export default function Home(){
    return(
        <main className="main-with-aside">
            <div class="flow">
                <h1>This is the Home Page!</h1>
            </div>
        <Aside/>
        </main>
    );
}