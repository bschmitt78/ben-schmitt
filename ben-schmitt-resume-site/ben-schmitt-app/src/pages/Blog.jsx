import Aside from "../components/aside";

export default function Blog(){
    return(
        <main className="main-with-aside">
            <div class="flow">
                <h1>This is the Blog Page!</h1>
            </div>
        <Aside/>
        </main>
    );
}