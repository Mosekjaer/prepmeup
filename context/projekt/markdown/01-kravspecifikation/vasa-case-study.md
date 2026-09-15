# Why The Vasa Sank: 10 Lessons Learned

## Metadata

| Felt | Værdi |
|---|---|
| **Lektion** | L2 — Kravspecifikation (forberedelseslæsning: "Læs VasaCase før lektion") |
| **Kursus** | SWISE-01 Indledende System Engineering |
| **Kilde** | `System Specification VasaCaseStudy.pdf` (7 sider) |
| **Type** | artikel |
| **Forfatter** | Richard E. (Dick) Fairley, Oregon Graduate Institute |
| **Emner dækket** | Requirements creep, manglende specifikationer, schedule pressure, manglende projektplan, ignoreret test, kommunikation — som case for systemspecifikation |

Fuld tekst nedenfor (artiklen er ren tekst uden figurer).

---

## Introduction

Around 4:00 PM on August 10th, 1628 the warship Vasa set sail in Stockholm harbor on its maiden voyage as the newest ship in the Royal Swedish Navy. After sailing about 1300 meters, a light gust of wind caused the Vasa to heel over on its side. Water poured in through the gun portals and the ship sank with a loss of 53 lives. The Vasa lay in shallow waters of Stockholm harbor (at 32 meters depth) and after initial attempts to salvage it failed, was largely forgotten until it was located by Anders Franzen in 1956 [1]. In 1961, 333 years after it sank, the Vasa was raised and was so well preserved that it could float after the gun portals were sealed and water and mud were pumped from it. Today it is housed in a museum specially built for it, near the site where it foundered [6].

That the Vasa is so remarkably well preserved is based on two factors: the sheltered harbor in which the Vasa lay, and the salinity of the water in the Baltic Sea. Because it lay in a sheltered harbor, the Vasa was protected from storms that would otherwise have destroyed it in the shallow waters of the Baltic Sea. Because of the salinity of the water, worms that would otherwise have infested and destroyed the wooden vessel are not present in the Baltic.

The sinking of the Vasa was a major disaster for Sweden. The country was at war with Poland and the ship was needed for the war effort. No expense had been spared. The Vasa was the most expensive project ever undertaken by Sweden and it was a total loss. The ship's captain survived the sinking and was immediately thrown into jail. On August 11th, the day after the disaster, a preliminary board of inquiry was convened. Incompetence of the captain and crew was ruled out and the captain was set free. A formal hearing was conducted in September of 1628. No exact reason for the sinking was determined and no one was blamed.

Since being salvaged in 1961, the Vasa has been extensively analyzed and historical records concerning its construction have been examined. The fundamental reason the Vasa sank is, of course, that the ship was unstable. The reasons that the Vasa was constructed to be unstable, and launched when known to be unstable, are numerous and varied. The lessons to be learned are as relevant to our modern-day attempts to build large, complex systems as they were to the art and craft of building warships in 1628. The story of the Vasa unfolds as follows.

> Side 1

## The King frequently changed his orders for ships to be built.

On January 16, 1625, King Gustav II Adolph of Sweden directed Admiral Fleming to sign a contract with the Stockholm ship builders Henrik and Arend Hybertsson to design and oversee construction of four ships. Henrik was the master shipwright and Arend was the business manager. They subsequently subcontracted with shipbuilder Johan Isbrandsson to construct the ships under their direction. The four ships were to be built over a period of four years: two smaller ones having keel lengths of about 108 feet and two larger ones having keel lengths of about 135 feet.

Based on a series of on-going (and confusing) changes ordered by the King during the spring and summer of 1625, Henrik requested oak timbers be cut from the King's forest for two 108-foot ships and one 135-foot ship. On September 20, 1625 the Swedish Navy lost ten ships in a devastating storm. The King then ordered that the two smaller ships be built first on an accelerated schedule to replace two of the lost ships. Construction of the Vasa commenced in early 1626, as a small ship, and was completed 2 ½ years later, in August of 1628 as a large ship, after undergoing numerous changes in requirements.

On November 30, 1625 the King changed the order for the two smaller ships, requiring them to be 120-foot in length. The ships were to be enlarged so that more armament could be carried. They were to each carry thirty-two 24-pound guns in a traditional enclosed deck configuration¹. An inventory of materials by Henrik Hybertsson indicated that he had enough timbers available to construct one 111-foot ship (an approximation to 120-foot based on availability of materials) and one 135-foot ship. Under the King's direction, as conveyed by Admiral Fleming, Henrik laid the keel for a 111-foot ship because it could be completed more quickly than the 135-foot ship (for which timbers were also available). It is not clear from the records whether the keel for a 108-foot ship had already been laid and was then extended to 111-foot, or whether the 111-foot keel was laid initially.

> Side 1–2

## No specifications for construction of the Vasa's modified keel were prepared.

After the 111-foot keel of the Vasa had been laid, King Gustav learned that a large ship with two gun decks was being built in Denmark. This resulted in an order from the King that the ship currently under construction (the Vasa) be enlarged and that the enlarged ship have two enclosed gun decks. Admiral Fleming thus relayed the order for the 111-foot ship to be scaled up to 135 feet and that a second enclosed gun deck be added. Scaling-up the 111-foot keel using materials planned for the 135-foot ship was thought to be more expeditious than laying a new 135-foot keel. It is worth of note that when King Gustav ordered the Vasa be scaled up to a larger ship no one in Sweden, and Hybertsson in particular, had ever built a ship with two enclosed gun decks.

It is also noteworthy that the evolution of warship architecture from one enclosed gun deck to two enclosed gun decks marked a change in warfare tactics that became commonplace in the late 1600s and 1700s. In the 1500s and early 1600s, the cannons on warships were used to fire initial volleys with the main objective being to cripple the opponent's ship so that it could be boarded and seized. To this end, the earlier warships carried large numbers of soldiers (as many as 300). With the introduction of two-gun-deck warships, the objective became to fire broadside volleys and sink the opponent.

The contract with the Hybertssons was revised but no specifications, or crude sketches, for the Vasa (in either the 111-foot or 135-foot version) have ever been found. It is not likely that specifications would have been prepared for the original 108-foot version of the Vasa because these types of ships had been routinely built for many years and Hybertsson was an experienced shipwright, working with an experienced ship builder. None of the related (and well-preserved) documents mentions drawings for the larger versions of the Vasa. Given the circumstances and the schedule pressure under which the Vasa was constructed it is most likely that time was not spent to prepare specifications for the larger versions of the Vasa. It is likely that Henrik Hybertsson "scaled up" the dimensions of the original 108 foot ship to meet the length and breadth requirements of the 111-foot ship and subsequently scaled those up for the 135-foot version of the Vasa.

¹ 24 pounds refers to the weight of the shot fired by the cannon. In those days, naval cannons were made of brass. A 24-pounder weighed approximately 3000 pounds.

The manner in which the 111-foot version of the Vasa was scaled up to 135-foot was constrained by the existing 111-foot keel. Examination of the Vasa's keel indicates that the keel for the 135-foot version of the ship was "scarfed" on to the keel for the 111-foot ship. The Vasa has 4 scarf joints as compared to the traditional three; the length of the Vasa's first three scarfs is 111 foot. With the addition of the fourth scarf the keel of the Vasa is 135 feet in length. As a result of the "scaling-up" the keel is thin in relation to its length and the depth of the keel is quite shallow for a 135-foot ship.

Hein Jacobsson (Hybertsson's assistant) later said that the Vasa was built one foot, five inches wider than originally planned. However, the keel was already laid, so the change in width could only be applied to the upper parts of the ship. This resulted in a high center of gravity and contributed to the instability of the Vasa (sailing ships are extremely sensitive to the location of the center of gravity; a few centimeters can make a large difference). It was discovered during outfitting of the ship that the shallow keel did not provide sufficient space in the hold for the amount of ballast needed to stabilize a 135-foot ship. Also, the thinness of the keel required extra bracing timbers in the hold, which further restricted the space available for ballast.

> Side 2–3

## Requirements for the armaments were changed repeatedly.

The numbers and types of armaments to be carried by the scaled-up Vasa went through a number of revisions. Initially, the 111-foot version of the Vasa was to carry thirty-two 24-pound guns. Then, the 135-foot version was to carry thirty-six 24-pound guns, twenty-four 12-pound guns, eight 48-pound mortars, and ten smaller guns. After a series of further revisions, the Vasa was to carry thirty 24-pounders on the lower deck and thirty 12-pounders on the upper deck. Finally, it was ordered that the Vasa carry sixty-four 24-pound guns; thirty-two on each deck plus several smaller guns (some documents state the required number as sixty 24-pound guns). Mounting only 24-pound guns had the advantage of providing more firepower, and allowed standardization on one kind of ammunition, gun carriage, powder charge, and other fittings.

The disadvantage was that the upper deck had to carry the added weight of the 24-pound guns in cramped space that had been built for 12-pound guns, which further raised the center of gravity of the ship. In the end, the Vasa was launched with forty-eight 24-pound guns; (twenty four on each deck) because manufacturing problems of the gun supplier prevented delivery of more guns on schedule. Waiting for the additional guns would have interfered with the requirement to launch the ship as soon as possible. Another indication of excessive schedule pressure is that recent examination of the guns (post-1961) indicates the casting were of poor quality. The guns may well have malfunctioned (exploded) during a naval battle.

The Vasa's rigging and outfitting were built by artisan shipbuilders, without explicit specifications or plans, in the traditional manner that had evolved over many years. The King ordered that the ship be outfitted with hundreds of ornate, gilded and painted carvings depicting Biblical, mythical, and historical themes. The Vasa was meant to impress by outdoing the Danish ship being built; no cost was spared. The Vasa was the most expensive ship of its time. However, the heavy oak carvings raised the center of gravity and further contributed to the instability of the Vasa.

> Side 3–4

## Henrik Hybertsson (the shipwright) became ill and died in 1627.

Henrik Hybertsson became seriously ill in 1626 and died in 1627, one year before the Vasa was completed. During the year of his illness, he shared supervision of the project with his assistant, Hein Jacobsson, and the shipbuilder Johan Isbrandsson. Jacobsson was made responsible for completing the project after Hybertsson's death. According to historical records, management of the Vasa project was weak after Hybertsson became ill. Division of responsibility was not clear during his illness, and because there were no detailed specifications, schedule milestones, or work plans it was difficult for Jacobsson to understand and implement Hybertsson's undocumented plans. Communication among Hybertsson, Jacobsson, and Isbrandsson was poor. This resulted in further delays in completion of the ship.

At the time of Hybertsson's death and during the subsequent year, four hundred people in five different groups were working on the hull, the carvings, the rigging, the armaments, and the ballasting, apparently without any communication or coordination among them. This was the largest work force ever engaged in a single project in Sweden up to that time. There is no evidence that Jacobsson made any documented plans after becoming responsible for completion of the ship.

> Side 4

## There were no known methods for calculating factors such as stability, stiffness, and sailing characteristics of ships.

Methods of calculating the center of gravity, the heeling characteristics, and stability factors for sailing ships were unknown. As a consequence, ships' captains had to learn the operational characteristics of their ships by trial-and-error testing. Vasa was the most spectacular, but certainly not the only, ship to sink by heeling over during the 17th and 18th centuries.

Measurements taken and calculations performed since 1961 indicate that the Vasa was so unstable it would have heeled over at a list of 10 degrees; it could not have withstood the wind gust of 8 knots that caused the ship to capsize (8 knots — about 9 mph — being the estimated speed of the gust that caused the Vasa to sink) [2]. Recent calculations indicate the ship would have heeled over in a breeze of 4 knots.

That the wind was so light is verified by the fact that the crew had to extend the sails by hand upon launch. Lieutenant Petter Gierdsson testified at the formal inquiry held in September of 1628: "The weather was not strong enough to pull out the sheets, although the blocks were well lubricated. Therefore, they had to push the sheets out, and one man was enough to hold a sheet" [4].

During the formal inquiry, several witnesses commented that the Vasa was "heavier above than below," but no one pursued the questions of how or why the Vasa had become top-heavy. There was no mentioned of the weight of the second deck, the guns, the carvings, or other equipment. In those days, most people (including the experts) thought that the higher and more impressive a warship, and the more and bigger the guns it carried, the more indestructible it would be.

> Side 4

## A stability test conducted before launching the Vasa showed that the ship was not seaworthy.

A stability test was conducted in the presence of Admiral Fleming and Captain Hannson (the Vasa's captain) during outfitting of the Vasa. The test consisted of having 30 men run from side-to-side amidship (a "lurch" test). After three traversals by the men, the test was halted because the ship was rocking so violently it was feared it would heel over. The ship could not be stabilized because there was no room for additional ballast under the floorboards in the hold. Had additional ballast been added, the added weight would have placed the lower-deck gun portals near or below the waterline of the ship. It is estimated that the Vasa was carrying about 120 tons of ballast. More than twice that amount would have been needed to stabilize the ship.

That the Vasa was launched with known stability problems is the result of poor communication, pressure from King Gustav to launch the ship as soon as possible, the fact that the King was in Poland conducting a war campaign, and because no one had any suggestions for making the ship more stable.

Testimony at the formal hearing held in September, 1628 indicates that Jacobsson, the shipwright, and Isbrandsson, the shipbuilder, were not present during the stability test and were unaware of the outcome. The boatswain, Matsson, testified that Admiral Fleming had accused him of carrying too much ballast. According to Matsson, Admiral Fleming had said "You are carrying too much ballast: the gunports are too close to the water!." Mattson then claimed to have answered: "God grant that the ship will stand upright on her keel." To which the Admiral replied: "the shipbuilder has built ships before and you should not be worried." [2].

Whether Admiral Fleming and Captain Hannson intentionally withheld the results of the stability test is a matter for speculation. It is known that the King had ordered that the Vasa be ready by July 25th and "if not, those responsible would be subject to His Majesty's disgrace." The maiden voyage of the Vasa on August 10th was more than two weeks later than ordered by the King. It was reported that after the failed stability test, Admiral Fleming lamented "If only the King were here." [3].

> Side 5

## Lessons-Learned

The lessons to be learned from the sinking of the Vasa are as relevant today as in 1628. Those lessons are summarized as follows:

1. **Excessive schedule pressure:** The Vasa was completed under strong time constraints to meet a pressing need.
2. **Changing needs:** Many changes to operational characteristics were made during construction of the ship.
3. **Lack of technical specifications:** The (non-existent) specifications were not revised as the operational requirements changed.
4. **Lack of a documented project plan:** During a year-long transition in leadership it was difficult for the assistant to manage the project. This resulted in poor supervision of the various groups working on the ship (i.e., the shipwright, the ship builder, and the numerous subcontractors). There is no evidence that the new project manager (the former assistant) prepared any plans after the original shipwright died.
5. **Excessive innovation:** No one in Sweden, including the shipwright, had ever built a ship having two gun decks.
6. **Secondary innovations:** Many secondary innovations were added during construction of the Vasa to accommodate the increased length, the additional gun deck, and other changes.
7. **Requirements creep:** It seems that no one was aware of the degree to which the Vasa had evolved during the 2 ½ years of construction.
8. **Lack of scientific methods:** There were no known methods for calculating center of gravity, stiffness, and the resulting stability relationships of the Vasa.
9. **Ignoring the obvious:** The Vasa was launched after failing a stability test.
10. **Possible mendacity:** Results of the stability test were known to some but were not communicated to others.

> Side 5–6

## Summary

It is clear that the Vasa was planned as a small, traditional ship but became a large, innovative ship intended to carry maximum armament without regard for factors such as stability, stiffness, and sailing characteristics. The result was a ship that was not seaworthy. The Vasa had insufficient ballast for stability in a light breeze and adding sufficient ballast (had there been room for it) would have put the lower-deck gun portals at or below the waterline.

According to the transcript of the formal hearing, no one inquired as to how or why the Vasa had become unstable or why the Vasa was launched with known stability problems. The failure of this line of inquiry is perhaps the most compelling of the lessons to be learned from the Vasa.

To end on a positive note, it should be observed that large, two-deck warships were subsequently built and sailed during the latter 17th, 18th, and 19th centuries.

## Acknowledgement

While we may never know the exact details of events presented here, this article depicts the author's "most probable scenario" based on evidence collected during visits to the Vasa museum, to the Web sites, and in publications by those who have investigated the circumstances of the Vasa's sinking.

## Biography

Richard E. (Dick) Fairley is a professor of computer science and director of the software engineering program at the Oregon Graduate Institute. He also teaches in the Oregon Master of Software Engineering degree program, offered collaboratively by four Oregon universities. His research interests include software estimation, project management, software process modeling, risk management, software architecture, and software engineering education. Contact him at the OGI School of Science and Engineering, 20000 N.W. Walker Rd., Beaverton, OR 97006 or at d.fairley@computer.org.

> Side 6

## References

1. A. Franzen, *The Warship Vasa*, Stockholm, Sweden, 1974.
2. C. Borgenstam and A. Sandstrom, *Why Vasa Capsized*, AB Grafisk Press, Stockholm, Sweden, 1995.
3. Documentation available in the Vasa Museum, Stockholm, Sweden.
4. *The Story of the Royal Swedish Man-of-War Vasa*, multi-media documentary, VyKett AB, Stockholm, Sweden, 1999.
5. *Warship Vasa*, a documentary film by Anders Wahlgren, Vasamuseet, Stockholm, Sweden, 1996.
6. www.vasamuseet.se and home.swipnet.se/~w-70853/WASAe.htm

> Side 7
